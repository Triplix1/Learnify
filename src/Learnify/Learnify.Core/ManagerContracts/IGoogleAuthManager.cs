using Google.Apis.Auth;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Dto.Auth.ExternalDto;

namespace Learnify.Core.ManagerContracts;

public interface IGoogleAuthManager
{
    Task<GoogleTokenResult> GetGoogleTokenAsync(GoogleAuthRequest googleAuthRequest);
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
}