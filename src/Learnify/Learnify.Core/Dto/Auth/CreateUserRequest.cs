using Learnify.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace Learnify.Core.Dto.Auth;

public class CreateUserRequest
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
    /// Gets or sets value for Password
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Gets or sets value for Role
    /// </summary>
    public Role Role { get; set; }
}