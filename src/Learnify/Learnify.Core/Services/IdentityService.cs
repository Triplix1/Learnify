using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Google.Apis.Auth;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Domain.RepositoryContracts.UnitOfWork;
using Learnify.Core.Dto;
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

    public async Task<ApiResponse<AuthResponse>> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest)
    {
        GoogleJsonWebSignature.Payload tokenPayload;
        try
        {
            var googleToken = await _googleAuthManager.GetGoogleTokenAsync(googleAuthRequest);

            tokenPayload = await _googleAuthManager.VerifyGoogleToken(googleToken.IdToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return ApiResponse<AuthResponse>.Failure(e);
        }
        

        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(tokenPayload.Email);

        if (user is null)
        {
            _logger.LogInformation("Cannot find user with email: {Email}. Creating it", tokenPayload.Email);

            user = new User()
            {
                Email = tokenPayload.Email,
                Role = _mapper.Map<Role>(googleAuthRequest.Role),
                IsGoogleAuth = true
            };

            await _psqUnitOfWork.UserRepository.CreateAsync(user);
        }

        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var tokenData = _tokenManager.GetDataFromToken(refreshTokenRequest.Jwt);

        if (tokenData.ValidTo > DateTime.UtcNow)
        {
            _logger.LogError("Token hasn't expired yet");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Token hasn't expired yet"));
        }

        var email = tokenData.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (email is null)
        {
            _logger.LogError("Cannot find email in token principals");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Cannot find email in token"));
        }

        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(email);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", email);
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException($"Cannot find user with email: {email}"));
        }

        var refreshToken = await _psqUnitOfWork.RefreshTokenRepository.GetByJwtAsync(refreshTokenRequest.Jwt);

        if (refreshToken is null)
        {
            _logger.LogError("Cannot find refresh token for specified jwt");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Cannot find refresh token for specified jwt"));
        }

        if (refreshToken.Refresh != refreshTokenRequest.RefreshToken)
        {
            _logger.LogError("Invalid refresh token for specified jwt");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Invalid refresh token for specified jwt"));
        }

        if (refreshToken.Expire < DateTime.UtcNow)
        {
            _logger.LogError("Refresh token has been expired");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Refresh token has been expired"));
        }

        if (refreshToken.HasBeenUsed)
        {
            _logger.LogError("Refresh token has been used");
            return ApiResponse<AuthResponse>.Failure(new RefreshTokenException("Refresh token has been used"));
        }

        refreshToken.HasBeenUsed = true;
        await _psqUnitOfWork.RefreshTokenRepository.UpdateAsync(refreshToken);

        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest)
    {
        var userWithTheSameEmail = await _psqUnitOfWork.UserRepository.GetByEmailAsync(registerRequest.Email);

        if (userWithTheSameEmail is not null)
        {
            _logger.LogError("Cannot register user with email: {email}, because it already exists",
                registerRequest.Email);
            return ApiResponse<AuthResponse>.Failure(new ArgumentException("User with the same email already exists"));
        }

        var userWithTheSameUsername = await _psqUnitOfWork.UserRepository.GetByUsernameAsync(registerRequest.Username);

        if (userWithTheSameUsername is not null)
        {
            _logger.LogError("Cannot register user with username: {username}, because it already exists",
                registerRequest.Username);
            return ApiResponse<AuthResponse>.Failure(new ArgumentException("User with the same username already exists"));
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
            var stream = registerRequest.Image.OpenReadStream();

            var imageBlobAddRequest = new BlobDto()
            {
                Name = user.Email,
                Content = stream,
                ContainerName = "indentity-user-images",
                ContentType = "image/*"
            };

            var imageBlob = await _blobStorage.UploadAsync(imageBlobAddRequest);

            user.ImageUrl = imageBlob.Url;
            user.ImageBlobName = imageBlob.Name;
            user.ImageContainerName = imageBlob.ContainerName;
        }

        var createdUser = await _psqUnitOfWork.UserRepository.CreateAsync(user);
        
        return await ReturnNewAuthResponseAsync(user);
    }

    public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _psqUnitOfWork.UserRepository.GetByEmailAsync(loginRequest.Email);

        if (user is null)
        {
            _logger.LogError("Cannot find user with email: {email}", loginRequest.Email);
            return ApiResponse<AuthResponse>.Failure(new AuthenticationException("Cannot find user with such email"));
        }

        if (user.PasswordSalt is null || user.PasswordHash is null)
        {
            _logger.LogError("Trying to login user with email: {email}, but it doesn't registered with password",
                loginRequest.Email);
            return ApiResponse<AuthResponse>.Failure(
                new AuthenticationException("Your user doesn't registered with password type"));
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginRequest.Password));

        if (computedHash.Where((t, i) => t != user.PasswordHash[i]).Any())
        {
            _logger.LogError("Login faild, invalid password for user: {email}", loginRequest.Email);
            return ApiResponse<AuthResponse>.Failure(new AuthenticationException("Invalid Password"));
        }

        return await ReturnNewAuthResponseAsync(user);
    }

    private async Task<ApiResponse<AuthResponse>> ReturnNewAuthResponseAsync(User user)
    {
        TokenResponse accessToken;
        
        try
        {
            accessToken = _tokenManager.GenerateJwtToken(user);
        }
        catch (Exception e)
        {
            return ApiResponse<AuthResponse>.Failure(e);
        }
        
        var refreshTokenString = _tokenManager.GenerateRefreshToken();

        var refreshToken = new RefreshToken()
        {
            Expire = DateTime.UtcNow.AddHours(3),
            Jwt = accessToken.Token,
            Refresh = refreshTokenString,
            HasBeenUsed = false
        };

        await _psqUnitOfWork.RefreshTokenRepository.CreateAsync(refreshToken);

        var response = new AuthResponse()
        {
            Token = accessToken.Token,
            RefreshToken = refreshTokenString,
            Expires = accessToken.Expires
        };

        return ApiResponse<AuthResponse>.Success(response);
    }
}