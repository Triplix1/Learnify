namespace Profile.Core.Options;

/// <summary>
/// Jwt options
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Gets or sets value for Key
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// Gets or sets value for Expire
    /// </summary>
    public TimeSpan Expire { get; set; }
    /// <summary>
    /// Gets or sets value for Audience
    /// </summary>
    public string Audience { get; set; }
    /// <summary>
    /// Gets or sets value for Issuer
    /// </summary>
    public string Issuer { get; set; }
}