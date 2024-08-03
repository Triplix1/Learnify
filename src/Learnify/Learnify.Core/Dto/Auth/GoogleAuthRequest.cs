using Learnify.Core.Enums;

namespace Learnify.Core.Dto.Auth;

/// <summary>
/// GoogleAuthRequest
/// </summary>
public class GoogleAuthRequest
{
    /// <summary>
    /// Gets or sets value for Code
    /// </summary>
    public string Code { get; set; }
    
    /// <summary>
    /// Gets or sets value for CodeVerifier
    /// </summary>
    public string CodeVerifier { get; set; }
    
    /// <summary>
    /// Gets or sets value for RedirectUrl
    /// </summary>
    public string RedirectUrl { get; set; }
    
    /// <summary>
    /// Gets or sets value for Role
    /// </summary>
    public RoleRequest Role { get; set; }
}