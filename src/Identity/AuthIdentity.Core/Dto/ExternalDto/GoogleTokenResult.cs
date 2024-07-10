using Newtonsoft.Json;

namespace AuthIdentity.Core.ExternalDto;

public class GoogleTokenResult
{
    [JsonProperty("id_token")]
    public string IdToken { get; set; }
    
    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    
    [JsonProperty("access_type")]
    public string AccessType { get; set; }
    
    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; }
    
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}