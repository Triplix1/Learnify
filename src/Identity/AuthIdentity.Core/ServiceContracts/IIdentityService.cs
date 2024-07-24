using AuthIdentity.Core.Dto;
using General.Dto;

namespace AuthIdentity.Core.ServiceContracts;

public interface IIdentityService
{
    Task<ApiResponse<AuthResponse>> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest);
    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest);
    Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest loginRequest);
}