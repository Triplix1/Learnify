using System.Net.Http.Headers;
using Google.Apis.Auth;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Dto.Auth.ExternalDto;
using Learnify.Core.Exceptions;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Learnify.Core.Managers;

public class GoogleAuthManager : IGoogleAuthManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GoogleAuthManager> _logger;
    private readonly GoogleAuthOptions _googleAuthOptions;
    private const string GoogleTokenUrl = "https://oauth2.googleapis.com/token";

    public GoogleAuthManager(IOptions<GoogleAuthOptions> googleAuthOptions, IHttpClientFactory httpClientFactory,
        ILogger<GoogleAuthManager> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _googleAuthOptions = googleAuthOptions.Value;
    }

    public async Task<GoogleTokenResult> GetGoogleTokenAsync(GoogleAuthRequest googleAuthRequest,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, GoogleTokenUrl);

        var query = new Dictionary<string, string>()
        {
            { "grant_type", "authorization_code" },
            { "client_id", _googleAuthOptions.ClientId },
            { "client_secret", _googleAuthOptions.ClientSecret },
            { "code", googleAuthRequest.Code },
            { "access_type", "offline" },
            { "code_verifier", googleAuthRequest.CodeVerifier },
            { "redirect_uri", googleAuthRequest.RedirectUrl }
        };

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        request.Content = new FormUrlEncodedContent(query);

        var response = await _httpClientFactory.CreateClient().SendAsync(request, cancellationToken);
        var data = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError(
                "Error response from https://oauth2.googleapis.com/token, while trying to get token, message: {ResponseMessage}",
                data);
            throw new TokenExchangeException();
        }

        var tokenResult = JsonConvert.DeserializeObject<GoogleTokenResult>(data);

        if (tokenResult is null)
        {
            _logger.LogError("Token result from https://oauth2.googleapis.com/token is null");
            throw new TokenExchangeException();
        }

        _logger.LogDebug("Google token request completed successfully");
        return tokenResult;
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleTokenAsync(string idToken,
        CancellationToken cancellationToken = default)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>() { _googleAuthOptions.ClientId }
        };

        GoogleJsonWebSignature.Payload payload;

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (Exception)
        {
            _logger.LogError("Error while validating token");
            throw;
        }

        _logger.LogDebug("Token for user with email: {Email}, validated successfully", payload.Email);
        return payload;
    }
}