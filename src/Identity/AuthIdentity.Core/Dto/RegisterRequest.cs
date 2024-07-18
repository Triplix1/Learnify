using AuthIdentity.Core.Enums;
using Microsoft.AspNetCore.Http;

namespace AuthIdentity.Core.Dto;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public IFormFile? Image { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public RoleRequest Role { get; set; }
}