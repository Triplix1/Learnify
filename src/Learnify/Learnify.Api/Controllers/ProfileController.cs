using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto.Profile;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

/// <summary>
/// Profile controller
/// </summary>
[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IProfileService _profileService;

    /// <summary>
    /// Initializes a nwe instance of <see cref="ProfileController"/>
    /// </summary>
    /// <param name="profileService"></param>
    /// <param name="mapper"></param>
    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    /// <summary>
    /// Returns [rofile with specified Id
    /// </summary>
    /// <param name="id">Profile id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileResponse>> GetById([FromRoute]int id,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileService.GetByIdAsync(id, cancellationToken);
        return Ok(profile);
    }

    /// <summary>
    /// Updates profile
    /// </summary>
    /// <param name="profileUpdateRequest"><see cref="ProfileUpdateRequest"/></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<ActionResult<ProfileResponse>> Update(
        [FromForm]ProfileUpdateRequest profileUpdateRequest, CancellationToken cancellationToken = default)
    {
        var result = await _profileService.UpdateAsync(profileUpdateRequest, cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    /// Updates profile
    /// </summary>
    /// <param name="profileUpdateRequest"><see cref="ProfileUpdateRequest"/></param>
    /// <returns></returns>
    [HttpPut("update-role")]
    public async Task<ActionResult<ProfileResponse>> UpdateRole(
        [FromBody]UpdateUserRoleRequest profileUpdateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var result = await _profileService.UpdateRoleAsync(userId, profileUpdateRequest, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Deletes profile with specified id
    /// </summary>
    /// <param name="id">profile's id</param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute]int id,
        CancellationToken cancellationToken = default)
    {
        await _profileService.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}