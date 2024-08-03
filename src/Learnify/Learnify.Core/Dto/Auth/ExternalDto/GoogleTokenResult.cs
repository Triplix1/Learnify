using Newtonsoft.Json;

namespace Learnify.Core.Dto.Auth.ExternalDto;

/// <summary>
/// GoogleTokenResult
/// </summary>
public class GoogleTokenResult
{
    /// <summary>
    /// Gets or sets value for IdToken
    /// </summary>
    [JsonProperty("id_token")]
    public string IdToken { get; set; }
    
    /// <summary>
    /// Gets or sets value for Scope
    /// </summary>
    [JsonProperty("scope")]
    public string Scope { get; set; }
    
    /// <summary>
    /// Gets or sets value for TokenType
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    
    /// <summary>
    /// Gets or sets value for AccessType
    /// </summary>
    [JsonProperty("access_type")]
    public string AccessType { get; set; }
    
    /// <summary>
    /// Gets or sets value for ExpiresIn
    /// </summary>
    [JsonProperty("expires_in")]
    public string ExpiresIn { get; set; }
    
    /// <summary>
    /// Gets or sets value for RefreshToken
    /// </summary>
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}