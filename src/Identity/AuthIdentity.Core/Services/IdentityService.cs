using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using AuthIdentity.Core.Dto;
using AuthIdentity.Core.Enums;
using AuthIdentity.Core.Exceptions;
using AuthIdentity.Core.ManagerContracts;
using AuthIdentity.Core.ServiceContracts;
using AutoMapper;
using ClassLibrary1.Blob;
using Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AuthIdentity.Core.Services;

public class IdentityService : IIdentityService
{
    private readonly IGoogleAuthManager _googleAuthManager;
    private readonly ILogger<IdentityService> _logger;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<BlobAddRequest> _addBlobRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public IdentityService(IGoogleAuthManager googleAuthManager,
        ILogger<IdentityService> logger,
        ITokenService tokenService,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IMapper mapper,
        IRequestClient<BlobAddRequest> addBlobRequestClient,
        IPublishEndpoint publishEndpoint)
    {
        _googleAuthManager = googleAuthManager;
        _logger = logger;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
        _addBlobRequestClient = addBlobRequestClient;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<AuthResponse> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest)
    {
        var googleToken = await _googleAuthManager.GetGoogleTokenAsync(googleAuthRequest);

        var tokenPayload = await _googleAuthManager.VerifyGoogleToken(googleToken.IdToken);

        var user = await _userRepository.GetByEmailAsync(tokenPayload.Email);

        if (user is null)
        {
            _logger.LogInformation("Cannot find user with email: {Email}. Creating it", tokenPayload.Email);

            user = new User()
            {
                Email = tokenPayload.Email,
                Role = _mapper.Map<Role>(googleAuthRequest.Role),
                IsGoogleAuth = true
            };

            await _userRepository.CreateAsync(user);
        }

        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var tokenData = _tokenService.GetDataFromToken(refreshTokenRequest.Jwt);

        if (tokenData.ValidTo > DateTime.UtcNow)
        {
            _logger.LogError("Token hasn't expired yet");
            throw new RefreshTokenException("Token hasn't expired yet");
        }

        var email = tokenData.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (email is null)
        {
            _logger.LogError("Cannot find email in token principals");
            throw new RefreshTokenException("Cannot find email in token");
        }

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", email);
            throw new RefreshTokenException($"Cannot find user with email: {email}");
        }

        var refreshToken = await _refreshTokenRepository.GetByJwtAsync(refreshTokenRequest.Jwt);

        if (refreshToken is null)
        {
            _logger.LogError("Cannot find refresh token for specified jwt");
            throw new RefreshTokenException("Cannot find refresh token for specified jwt");
        }

        if (refreshToken.Refresh != refreshTokenRequest.RefreshToken)
        {
            _logger.LogError("Invalid refresh token for specified jwt");
            throw new RefreshTokenException("Invalid refresh token for specified jwt");
        }

        if (refreshToken.Expire < DateTime.UtcNow)
        {
            _logger.LogError("Refresh token has been expired");
            throw new RefreshTokenException("Refresh token has been expired");
        }

        if (!refreshToken.HasBeenUsed)
        {
            _logger.LogError("Refresh token has been used");
            throw new RefreshTokenException("Refresh token has been used");
        }

        refreshToken.HasBeenUsed = true;
        await _refreshTokenRepository.UpdateAsync(refreshToken);

        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var userWithTheSameEmail = await _userRepository.GetByEmailAsync(registerRequest.Email);

        if (userWithTheSameEmail is not null)
        {
            _logger.LogError("Cannot register user with email: {email}, because it already exists",
                registerRequest.Email);
            throw new ArgumentException("User with the same email already exists");
        }

        var userWithTheSameUsername = await _userRepository.GetByUsernameAsync(registerRequest.Username);

        if (userWithTheSameUsername is not null)
        {
            _logger.LogError("Cannot register user with username: {username}, because it already exists",
                registerRequest.Username);
            throw new ArgumentException("User with the same username already exists");
        }

        using var hmac = new HMACSHA512();

        var user = new User()
        {
            Email = registerRequest.Email,
            Name = registerRequest.Name,
            Surname = registerRequest.Surname,
            Username = registerRequest.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerRequest.Password)),
            PasswordSalt = hmac.Key,
            Role = _mapper.Map<Role>(registerRequest.Role)
        };

        if (registerRequest.Image is not null)
        {
            using var memoryStream = new MemoryStream();
            await registerRequest.Image.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var imageBlobAddRequest = new BlobAddRequest()
            {
                Name = user.Email,
                Content = fileBytes,
                ContainerName = "indentity-user-images"
            };

            var imageBlob = await _addBlobRequestClient.GetResponse<BlobCreatedResponse>(imageBlobAddRequest);

            user.ImageUrl = imageBlob.Message.Url;
        }

        var createdUser = await _userRepository.CreateAsync(user);
        await _userRepository.SaveChangesAsync();

        var userCreatedMessage = _mapper.Map<UserCreated>(createdUser);
        await _publishEndpoint.Publish(userCreatedMessage);

        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", loginRequest.Email);
            throw new AuthenticationException("Cannot find user with such email");
        }

        if (user.PasswordSalt is null || user.PasswordHash is null)
        {
            _logger.LogError("Trying to login user with email: {email}, but it doesn't registered with password",
                loginRequest.Email);
            throw new AuthenticationException("Your user doesn't registered with password type");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
        {
            _logger.LogError("Login faild, invalid password for user: {email}", loginRequest.Email);
            throw new AuthenticationException("Invalid Password");
        }

        return await ReturnNewAuthResponseAsync(user);
    }

    private async Task<AuthResponse> ReturnNewAuthResponseAsync(User user)
    {
        var accessToken = _tokenService.GenerateJwtToken(user);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken()
        {
            Expire = DateTime.UtcNow.AddHours(3),
            Jwt = accessToken,
            Refresh = refreshTokenString,
            HasBeenUsed = false
        };

        await _refreshTokenRepository.CreateAsync(refreshToken);
        await _userRepository.SaveChangesAsync();

        return new AuthResponse()
        {
            Token = accessToken,
            RefreshToken = refreshTokenString
        };
    }
}