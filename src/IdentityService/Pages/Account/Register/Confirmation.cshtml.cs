using IdentityService.DTOs.TemporaryUser;
using IdentityService.Models;
using IdentityService.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Confirmation : PageModel
{
    [BindProperty]
    public ConfirmationInputModel Input { get; set; }

    private readonly IConfirmationService _confirmationService;
    private readonly ITemporaryUserService _temporaryUserService;

    public Confirmation(IConfirmationService confirmationService, ITemporaryUserService temporaryUserService)
    {
        _confirmationService = confirmationService;
        _temporaryUserService = temporaryUserService;
    }

    public IActionResult OnGet([FromQuery]string returnUrl, [FromQuery]string email)
    {
        Input = new ConfirmationInputModel()
        {
            Email = email,
            ReturnUrl = returnUrl
        };
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();
        
        var confirmation = await _confirmationService.GetByEmailAsync(Input.Email);

        if (confirmation is null)
        {
            ModelState.AddModelError(nameof(Input.Code), "Your confirmation code has been retired");
            return Page();
        }

        if (Input.Code != confirmation.Code) return Page();
            
        await _confirmationService.DeleteAsync(confirmation.Id);

        var user = new TemporaryUserAddRequest()
        {
            Email = Input.Email
        };

        var result = await _temporaryUserService.CreateAsync(user);
        
        return RedirectToPage($"Data", new {userId = result.Id, returnUrl = Input.ReturnUrl});
    }
}