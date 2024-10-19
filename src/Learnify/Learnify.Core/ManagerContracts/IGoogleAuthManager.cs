using Google.Apis.Auth;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Dto.Auth.ExternalDto;

namespace Learnify.Core.ManagerContracts;

public interface IGoogleAuthManager
{
    Task<GoogleTokenResult> GetGoogleTokenAsync(GoogleAuthRequest googleAuthRequest,
        CancellationToken cancellationToken = default);

    Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken,
        CancellationToken cancellationToken = default);
}