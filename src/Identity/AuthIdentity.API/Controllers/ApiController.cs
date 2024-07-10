using AuthIdentity.Core.Dto;
using AuthIdentity.Core.ServiceContracts;
using General.Dto;
using General.Etensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthIdentityService.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public ApiController(IIdentityService identityService)
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
        public async Task<ActionResult<ApiResponse<AuthResponse>>> RegisterAsync([FromForm] RegisterRequest registerRequest)
        {
            var response = await _identityService.RegisterAsync(registerRequest);
            return Ok(response.ToApiResponse());
        }

        /// <summary>
        /// Login with Google.
        /// </summary>
        /// <param name="googleAuthRequest">The Google authentication request.</param>
        /// <returns>API response with authentication response.</returns>
        [HttpPost("external/google")]
        [SwaggerOperation(Summary = "Login with Google")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> LoginWithGoogleAsync([FromBody] GoogleAuthRequest googleAuthRequest)
        {
            var result = await _identityService.LoginWithGoogleAsync(googleAuthRequest);
            return Ok(result.ToApiResponse());
        }

        /// <summary>
        /// Login a user.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns>API response with authentication response.</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Login a user")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> LoginAsync([FromBody] LoginRequest loginRequest)
        {
            var result = await _identityService.LoginAsync(loginRequest);
            return Ok(result.ToApiResponse());
        }

        /// <summary>
        /// Refreshes the authentication token.
        /// </summary>
        /// <param name="refreshTokenRequest">The refresh token request.</param>
        /// <returns>API response with authentication response.</returns>
        [HttpPost("refresh")]
        [Authorize]
        [SwaggerOperation(Summary = "Refresh authentication token")]
        public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var refreshedTokenResponse = await _identityService.RefreshTokenAsync(refreshTokenRequest);
            return Ok(refreshedTokenResponse.ToApiResponse());
        }
    }
}

// using AuthIdentity.Core.Dto;
// using AuthIdentity.Core.ServiceContracts;
// using General.Dto;
// using General.Etensions;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using RefreshTokenRequest = AuthIdentity.Core.Dto.RefreshTokenRequest;
//
// namespace AuthIdentityService.Controllers;
//
// [Route("api")]
// public class ApiController : ControllerBase
// {
//     private readonly IIdentityService _identityService;
//
//     public ApiController(IIdentityService identityService)
//     {
//         _identityService = identityService;
//     }
//
//     [HttpPost("register")]
//     public async Task<ActionResult<ApiResponse<AuthResponse>>> RegisterAsync([FromForm] RegisterRequest registerRequest)
//     {
//         var response = await _identityService.RegisterAsync(registerRequest);
//
//         return Ok(response.ToApiResponse());
//     }
//     
//     [HttpPost("external/google")]
//     public async Task<ActionResult<ApiResponse<AuthResponse>>> LoginWithGoogleAsync([FromBody] GoogleAuthRequest googleAuthRequest)
//     {
//         var result = await _identityService.LoginWithGoogleAsync(googleAuthRequest);
//
//         return Ok(result.ToApiResponse());
//     }
//
//     [HttpPost("login")]
//     public async Task<ActionResult<ApiResponse<AuthResponse>>> LoginAsync([FromBody] LoginRequest loginRequest)
//     {
//         var result = await _identityService.LoginAsync(loginRequest);
//
//         return Ok(result.ToApiResponse());
//     }
//
//     [HttpPost("refresh")]
//     [Authorize]
//     public async Task<ActionResult<ApiResponse<AuthResponse>>> RefreshTokenAsync(
//         [FromBody]RefreshTokenRequest refreshTokenRequest)
//     {
//         var refreshedTokenResponse = await _identityService.RefreshTokenAsync(refreshTokenRequest);
//         return Ok(refreshedTokenResponse.ToApiResponse());
//     }
// }