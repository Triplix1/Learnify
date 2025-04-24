using Learnify.Core.Dto.Auth;

namespace Learnify.Core.ServiceContracts;

public interface IIdentityService
{
    // Task<AuthResponse> LoginWithGoogleAsync(GoogleAuthRequest googleAuthRequest,
    //     CancellationToken cancellationToken = default);

    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> RegisterUserAsync(RegisterModeratorRequest registerModeratorRequest,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> RegisterUserAsync(RegisterAdminRequest registerAdminRequest,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> RegisterUserAsync(RegisterRequest registerRequest,
        CancellationToken cancellationToken = default);

    Task<AuthResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken = default);
}