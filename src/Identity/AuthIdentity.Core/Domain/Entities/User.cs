using AuthIdentity.Core.Enums;
using General.Entities;

namespace AuthIdentity.Core.Domain.Entities;

public class User: BaseEntity
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string? ImageUrl { get; set; }
    public Role Role { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    
    public bool IsGoogleAuth { get; set; }

}