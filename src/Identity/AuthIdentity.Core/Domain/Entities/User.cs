using AuthIdentity.Core.Enums;

namespace AuthIdentity.Core.Domain.Entities;

public class User: BaseEntity
{
    public string Email { get; set; }
    public string Username { get; set; }
    public string? ImageUrl { get; set; }
    public Role Role { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    
    public bool IsGoogleAuth { get; set; }

}