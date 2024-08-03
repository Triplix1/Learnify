namespace Learnify.Core.Dto.Auth;

/// <summary>
/// Auth response
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Gets or sets value for Token
    /// </summary>
    public string Token { get; set; }
    /// <summary>
    /// Gets or sets value for RefreshToken
    /// </summary>
    public string RefreshToken { get; set; }
    /// <summary>
    /// Gets or sets value for Expires
    /// </summary>
    public DateTime Expires { get; set; }
}