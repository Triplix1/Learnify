using System.ComponentModel.DataAnnotations;
using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Enums;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class AuthController : BaseApiController
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> RegisterAsync(
        [Required] [FromBody]RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        var response = await _identityService.RegisterUserAsync(registerRequest, cancellationToken);
        return Ok(response);
    }

    [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.SuperAdmin)}")]
    [HttpPost("register-moderator")]
    public async Task<ActionResult<AuthResponse>> RegisterModeratorAsync(
        [Required] [FromBody]RegisterModeratorRequest registerRequest, CancellationToken cancellationToken = default)
    {
        var response = await _identityService.RegisterUserAsync(registerRequest, cancellationToken);
        return Ok(response);
    }

    // /// <summary>
    // /// Login with Google.
    // /// </summary>
    // /// <param name="googleAuthRequest">The Google authentication request.</param>
    // /// <returns>API response with authentication response.</returns>
    // [HttpPost("external/google")]
    // [SwaggerOperation(Summary = "Login with Google")]
    // public async Task<ActionResult<AuthResponse>> LoginWithGoogleAsync(
    //     [FromBody]GoogleAuthRequest googleAuthRequest, CancellationToken cancellationToken = default)
    // {
    //     var result = await _identityService.LoginWithGoogleAsync(googleAuthRequest, cancellationToken);
    //     return Ok(result);
    // }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody]LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _identityService.LoginAsync(loginRequest, cancellationToken);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> RefreshTokenAsync(
        [FromBody]RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default)
    {
        var refreshedTokenResponse =
            await _identityService.RefreshTokenAsync(refreshTokenRequest, cancellationToken);
        return Ok(refreshedTokenResponse);
    }
}