using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Register;

public class InputModel
{
    public string ReturnUrl { get; set; }
    [EmailAddress]
    [Required]
    public string Email { get; set; }
}