using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register;

public class Photo : PageModel
{
    public PhotoInputModel Input { get; set; }
    
    public void OnGet()
    {
        
    }
}