using Learnify.Core.Dto;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.ServiceContracts;

public interface IIdentityService
{
    Task<ApiResponse<AuthResponse>> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);
}