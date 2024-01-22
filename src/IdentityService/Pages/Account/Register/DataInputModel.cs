using System.ComponentModel.DataAnnotations;
using IdentityService.Models;

namespace IdentityService.Pages.Register;

public class DataInputModel
{
    public string ReturnUrl { get; set; }
    public Guid ClientId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public UserType Type { get; set; }
    public string? Company { get; set; }
    [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Count must be a natural number")]
    public string? CardNumber { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string ConfirmPassword { get; set; }
}