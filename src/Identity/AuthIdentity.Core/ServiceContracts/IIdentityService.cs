using AuthIdentity.Core.Dto;

namespace AuthIdentity.Core.ServiceContracts;

public interface IIdentityService
{
    Task<AuthResponse> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest);
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
    Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
}