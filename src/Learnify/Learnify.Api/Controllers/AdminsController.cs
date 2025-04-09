using JetBrains.Annotations;
using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto.Auth;
using Learnify.Core.Dto.Params;
using Learnify.Core.Dto.Profile;
using Learnify.Core.Enums;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

[Authorize(Roles = $"{nameof(Role.SuperAdmin)}")]
public class AdminsController : BaseApiController
{
    private readonly IProfileService _profileService;
    private readonly IIdentityService _identityService;

    public AdminsController(IProfileService profileService,
        IIdentityService identityService)
    {
        _profileService = profileService;
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> GetList(
        [FromQuery]PagedListParams paginatedParams,
        [FromQuery] [CanBeNull]OrderByParams orderByParams,
        CancellationToken cancellationToken = default)
    {
        var moderatorsListParams = new AdminsListParams()
        {
            PagedListParams = paginatedParams,
            OrderByParams = orderByParams,
        };

        var result = await _profileService.GetAdminsAsync(moderatorsListParams, cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> Create(
        RegisterAdminRequest registerAdminRequest,
        CancellationToken cancellationToken = default)
    {
        var result = await _identityService.RegisterUserAsync(registerAdminRequest, cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> Delete([FromRoute]int id,
        CancellationToken cancellationToken = default)
    {
        await _profileService.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}