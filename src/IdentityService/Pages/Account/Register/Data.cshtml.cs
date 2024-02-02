using AutoMapper;
using Contracts;
using IdentityService.Models;
using IdentityService.Services.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Data : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITemporaryUserService _temporaryUserService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public Data(UserManager<ApplicationUser> userManager, ITemporaryUserService temporaryUserService, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _userManager = userManager;
        _temporaryUserService = temporaryUserService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    [BindProperty]
    public DataInputModel Input { get; set; }
    
    public IActionResult OnGet(string returnUrl, Guid userId)
    {
        Input = new DataInputModel()
        {
            ClientId = userId,
            ReturnUrl = returnUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid) return Page();
        var user = await _temporaryUserService.GetByIdAsync(Input.ClientId);
        
        if (user == null)
        {
            ModelState.AddModelError(nameof(user.Id), "Cannot find user with such id");
            return Page();
        }

        if (Input.Password != Input.ConfirmPassword)
        {
            ModelState.AddModelError(nameof(Input.ConfirmPassword),"Password doesn't much");

            return Page();
        }

        var appUser = new ApplicationUser()
        {
            UserName = user.Email,
            Name = Input.Name,
            Surname = Input.Surname,
            Type = Input.Type,
            Company = Input.Company,
            CardNumber = Input.CardNumber,
            Email = user.Email,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(appUser, Input.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return Page();
        }
        
        await _userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("name", appUser.Name));
        await _userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("email", appUser.Email));
        await _userManager.AddClaimAsync(appUser, new System.Security.Claims.Claim("role", appUser.Type.ToString()));

        var userCreated = _mapper.Map<UserCreated>(appUser);

        await _publishEndpoint.Publish(userCreated);
            
        return RedirectToPage("/Account/Login/Index", new { returnUrl = Input.ReturnUrl });
    }
}