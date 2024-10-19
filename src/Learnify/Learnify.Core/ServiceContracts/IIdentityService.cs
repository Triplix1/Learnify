using Learnify.Core.Dto;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.ServiceContracts;

public interface IIdentityService
{
    Task<ApiResponse<AuthResponse>> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<AuthResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<AuthResponse>> RegisterAsync(RegisterRequest registerRequest,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<AuthResponse>>
        LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
}