using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Dto.Blob;
using Learnify.Core.Enums;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;
using Learnify.Core.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Learnify.Core.Services;

public class IdentityService : IIdentityService
{
    private readonly IGoogleAuthManager _googleAuthManager;
    private readonly ILogger<IdentityService> _logger;
    private readonly ITokenManager _tokenManager;
    private readonly IMapper _mapper;
    private readonly IBlobStorage _blobStorage;
    private readonly IPsqUnitOfWork _psqUnitOfWork;

    public IdentityService(IGoogleAuthManager googleAuthManager,
        ILogger<IdentityService> logger,
        ITokenManager tokenManager,
        IMapper mapper,
        IBlobStorage blobStorage,
        IPsqUnitOfWork psqUnitOfWork)
    {
        _googleAuthManager = googleAuthManager;
        _logger = logger;
        _tokenManager = tokenManager;
        _mapper = mapper;
        _blobStorage = blobStorage;
        _psqUnitOfWork = psqUnitOfWork;
    }

    public async Task<AuthResponse> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest,
        CancellationToken cancellationToken = default)
    {
        var googleToken = await _googleAuthManager.GetGoogleTokenAsync(googleAuthRequest, cancellationToken);

        var tokenPayload = await _googleAuthManager.VerifyGoogleTokenAsync(googleToken.IdToken, cancellationToken);


        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(tokenPayload.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogInformation("Cannot find user with email: {Email}. Creating it", tokenPayload.Email);

            user = new User()
            {
                Email = tokenPayload.Email,
                Role = _mapper.Map<Role>(googleAuthRequest.Role),
                IsGoogleAuth = true
            };

            await _psqUnitOfWork.UserRepository.CreateAsync(user, cancellationToken);
        }

        return await ReturnNewAuthResponseAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken = default)
    {
        var tokenData = _tokenManager.GetDataFromToken(refreshTokenRequest.Jwt);

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

        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", email);
            throw
                new RefreshTokenException($"Cannot find user with email: {email}");
        }

        var refreshToken =
            await _psqUnitOfWork.RefreshTokenRepository.GetByJwtAsync(refreshTokenRequest.Jwt, cancellationToken);

        if (refreshToken is null)
        {
            _logger.LogError("Cannot find refresh token for specified jwt");
            throw
                new RefreshTokenException("Cannot find refresh token for specified jwt");
        }

        if (refreshToken.Refresh != refreshTokenRequest.RefreshToken)
        {
            _logger.LogError("Invalid refresh token for specified jwt");
            throw
                new RefreshTokenException("Invalid refresh token for specified jwt");
        }

        if (refreshToken.Expire < DateTime.UtcNow)
        {
            _logger.LogError("Refresh token has been expired");
            throw new RefreshTokenException("Refresh token has been expired");
        }

        if (refreshToken.HasBeenUsed)
        {
            _logger.LogError("Refresh token has been used");
            throw new RefreshTokenException("Refresh token has been used");
        }

        refreshToken.HasBeenUsed = true;
        await _psqUnitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken, cancellationToken);

        return await ReturnNewAuthResponseAsync(user, cancellationToken);
    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterModeratorRequest registerModeratorRequest,
        CancellationToken cancellationToken = default)
    {
        var userCreateRequest = _mapper.Map<CreateUserRequest>(registerModeratorRequest);

        userCreateRequest.Role = Role.Moderator;
        
        return await RegisterUserAsync(userCreateRequest, cancellationToken);
    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterAdminRequest registerAdminRequest, CancellationToken cancellationToken = default)
    {
        var userCreateRequest = _mapper.Map<CreateUserRequest>(registerAdminRequest);

        userCreateRequest.Role = Role.Admin;
        
        return await RegisterUserAsync(userCreateRequest, cancellationToken);

    }

    public async Task<AuthResponse> RegisterUserAsync(RegisterRequest registerRequest,
        CancellationToken cancellationToken = default)
    {
        var userCreateRequest = _mapper.Map<CreateUserRequest>(registerRequest);
        
        userCreateRequest.Role = Role.Student;
        
        return await RegisterUserAsync(userCreateRequest, cancellationToken);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email, cancellationToken);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", loginRequest.Email);
            throw new AuthenticationException("Cannot find user with such email");
        }

        if (user.PasswordSalt is null || user.PasswordHash is null)
        {
            _logger.LogError("Trying to login user with email: {email}, but it doesn't registered with password",
                loginRequest.Email);
            throw
                new AuthenticationException("Your user doesn't registered with password type");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
        {
            _logger.LogError("Login failed, invalid password for user: {email}", loginRequest.Email);
            throw new AuthenticationException("Invalid Password");
        }

        return await ReturnNewAuthResponseAsync(user, cancellationToken);
    }

        private async Task<AuthResponse> RegisterUserAsync(CreateUserRequest createUserRequest,
        CancellationToken cancellationToken = default)
    {
        var userWithTheSameEmail =
            await _psqUnitOfWork.UserRepository.GetByEmailAsync(createUserRequest.Email, cancellationToken);

        if (userWithTheSameEmail is not null)
        {
            _logger.LogError("Cannot register user with email: {email}, because it already exists",
                createUserRequest.Email);
            throw new ArgumentException("User with the same email already exists");
        }

        var userWithTheSameUsername =
            await _psqUnitOfWork.UserRepository.GetByUsernameAsync(createUserRequest.Username, cancellationToken);

        if (userWithTheSameUsername is not null)
        {
            _logger.LogError("Cannot register user with username: {username}, because it already exists",
                createUserRequest.Username);
            throw new ArgumentException("User with the same username already exists");
        }

        using var hmac = new HMACSHA512();

        var user = new User()
        {
            Email = createUserRequest.Email,
            Name = createUserRequest.Name,
            Surname = createUserRequest.Surname,
            Username = createUserRequest.Username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(createUserRequest.Password)),
            PasswordSalt = hmac.Key,
            Role = createUserRequest.Role
        };

        if (createUserRequest.Image is not null)
        {
            var stream = createUserRequest.Image.OpenReadStream();

            var imageBlobAddRequest = new BlobDto()
            {
                Name = user.Email,
                Content = stream,
                ContainerName = "identity-user-images",
                ContentType = "image/*"
            };

            var imageBlob = await _blobStorage.UploadAsync(imageBlobAddRequest, cancellationToken: cancellationToken);

            user.ImageUrl = imageBlob.Url;
            user.ImageBlobName = imageBlob.Name;
            user.ImageContainerName = imageBlob.ContainerName;
        }

        user = await _psqUnitOfWork.UserRepository.CreateAsync(user, cancellationToken);

        return await ReturnNewAuthResponseAsync(user, cancellationToken);
    }

    private async Task<AuthResponse> ReturnNewAuthResponseAsync(User user,
        CancellationToken cancellationToken = default)
    {
        TokenResponse accessToken;

        accessToken = _tokenManager.GenerateJwtToken(user);

        var refreshTokenString = _tokenManager.GenerateRefreshToken();

        var refreshToken = new RefreshToken()
        {
            Expire = DateTime.UtcNow.AddDays(1),
            Jwt = accessToken.Token,
            Refresh = refreshTokenString,
            HasBeenUsed = false
        };

        await _psqUnitOfWork.RefreshTokenRepository.CreateAsync(refreshToken, cancellationToken);

        var response = new AuthResponse()
        {
            Token = accessToken.Token,
            RefreshToken = refreshTokenString,
            Expires = accessToken.Expires
        };

        return response;
    }
}