using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Register;

public class ConfirmationInputModel
{
    public string ReturnUrl { get; set; }
    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    public string Code { get; set; }
    public string Email { get; set; }
}