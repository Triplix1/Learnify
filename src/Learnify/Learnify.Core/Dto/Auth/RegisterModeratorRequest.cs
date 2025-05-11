using System.ComponentModel.DataAnnotations;

namespace Learnify.Core.Dto.Auth;

public class RegisterModeratorRequest
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }

}