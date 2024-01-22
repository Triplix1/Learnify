using Contracts;
using IdentityService.DTOs;
using IdentityService.Helpers;
using IdentityService.Models;
using IdentityService.Services.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; }

    private readonly IConfirmationService _confirmationService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPublishEndpoint _publishEndpoint;

    public Index(IConfirmationService confirmationService, UserManager<ApplicationUser> userManager, IPublishEndpoint publishEndpoint)
    {
        _confirmationService = confirmationService;
        _userManager = userManager;
        _publishEndpoint = publishEndpoint;
    }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl,
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();
        
        var containsConf = await _confirmationService.GetByEmailAsync(Input.Email);
        if (containsConf is not null)
            await _confirmationService.DeleteAsync(containsConf.Id);

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user is not null)
        {
            ModelState.AddModelError(nameof(Input.Email), "Here already consist user with such email");
            return Page();
        }
            
        var randomCode = CodeGeneratorHelper.Generate(4);
            
        var confirmationAddRequest = new ConfirmationAddRequest()
        {
            Code = randomCode,
            Email = Input.Email
        };
            
        await _confirmationService.CreateAsync(confirmationAddRequest);

        var emailRequest = new EmailRequest()
        {
            To = new[] { Input.Email },
            Content = $"<p>{randomCode}</p>",
            Subject = "Confirmation code"
        };

        await _publishEndpoint.Publish(emailRequest);

        return Redirect($"~/Account/Register/Confirmation?returnUrl={Input.ReturnUrl}&email={Input.Email}");

    }
}