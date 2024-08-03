using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Dto.Auth;

/// <summary>
/// RefreshTokenRequest
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Gets or sets value for Jwt
    /// </summary>
    [Required]
    public string Jwt { get; set; }
    
    /// <summary>
    /// Gets or sets value for RefreshToken
    /// </summary>
    [Required]
    public string RefreshToken { get; set; }
}