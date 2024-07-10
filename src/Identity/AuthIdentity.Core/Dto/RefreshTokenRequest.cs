using System.ComponentModel.DataAnnotations;

namespace AuthIdentity.Core.Dto;

public class RefreshTokenRequest
{
    [Required]
    public string Jwt { get; set; }
    
    [Required]
    public string RefreshToken { get; set; }
}