using System.ComponentModel.DataAnnotations;
using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Enums;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Learnify.Api.Controllers;

/// <summary>
/// AuthController
/// </summary>
[ApiController]
public class AuthController : BaseApiController
{
    private readonly IIdentityService _identityService;

    /// <summary>
    /// initializes a new instance of <see cref="AuthController"/>
    /// </summary>
    /// <param name="identityService"><see cref="IIdentityService"/></param>
    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The register request.</param>
    /// <returns>API response with authentication response.</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register a new user")]
    public async Task<ActionResult<AuthResponse>> RegisterAsync(
        [Required] [FromBody]RegisterRequest registerRequest, CancellationToken cancellationToken = default)
    {
        var response = await _identityService.RegisterUserAsync(registerRequest, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerRequest">The register request.</param>
    /// <returns>API response with authentication response.</returns>
    [Authorize(Roles = $"{nameof(Role.Admin)},{nameof(Role.SuperAdmin)}")]
    [HttpPost("register-moderator")]
    [SwaggerOperation(Summary = "Register a new user")]
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

    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="loginRequest">The login request.</param>
    /// <returns>API response with authentication response.</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login a user")]
    public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody]LoginRequest loginRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _identityService.LoginAsync(loginRequest, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the authentication token.
    /// </summary>
    /// <param name="refreshTokenRequest">The refresh token request.</param>
    /// <returns>API response with authentication response.</returns>
    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh authentication token")]
    public async Task<ActionResult<AuthResponse>> RefreshTokenAsync(
        [FromBody]RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken = default)
    {
        var refreshedTokenResponse =
            await _identityService.RefreshTokenAsync(refreshTokenRequest, cancellationToken);
        return Ok(refreshedTokenResponse);
    }
}