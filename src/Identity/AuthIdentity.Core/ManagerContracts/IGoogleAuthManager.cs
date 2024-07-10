using AuthIdentity.Core.Dto;
using AuthIdentity.Core.ExternalDto;
using Google.Apis.Auth;

namespace AuthIdentity.Core.ManagerContracts;

public interface IGoogleAuthManager
{
    Task<GoogleTokenResult> GetGoogleTokenAsync(GoogleAuthRequest googleAuthRequest);
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
}