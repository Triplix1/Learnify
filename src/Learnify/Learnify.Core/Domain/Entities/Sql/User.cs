using Learnify.Core.Enums;

namespace Learnify.Core.Domain.Entities.Sql;

/// <summary>
/// User entity
/// </summary>
public class User: BaseEntity<int>
{
    /// <summary>
    /// Gets or sets value for Email
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Gets or sets value for Name
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Gets or sets value for Surname
    /// </summary>
    public string Surname { get; set; }
    
    /// <summary>
    /// Gets or sets value for Username
    /// </summary>
    public string Username { get; set; }
    /// <summary>
    /// Gets or sets value for Company
    /// </summary>
    public string? Company { get; set; }
    
    /// <summary>
    /// Gets or sets value for CardNumber
    /// </summary>
    public string? CardNumber { get; set; }
    
    /// <summary>
    /// Gets or sets value for Role
    /// </summary>
    public Role Role { get; set; }
    
    /// <summary>
    /// Gets or sets value for PasswordHash
    /// </summary>
    public byte[]? PasswordHash { get; set; }
    
    /// <summary>
    /// Gets or sets value for PasswordSalt
    /// </summary>
    public byte[]? PasswordSalt { get; set; }
}