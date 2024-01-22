using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Register;

public class InputModel
{
    public string ReturnUrl { get; set; }
    [EmailAddress]
    public string Email { get; set; }
}