namespace Learnify.Core.Dto.Auth;

/// <summary>
/// LoginRequest
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets value for Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets value for Password
    /// </summary>
    public string Password { get; set; }
}