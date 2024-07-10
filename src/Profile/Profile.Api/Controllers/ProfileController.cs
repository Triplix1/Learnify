using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.DTO;
using Profile.Core.ServiceContracts;
using ProfileService.Controllers.Base;

namespace ProfileService.Controllers;

[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IProfileService _profileService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public ProfileController(IProfileService profileService, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _profileService = profileService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileResponse>> GetById([FromRoute]string id)
    {
        return Ok(await _profileService.GetByIdAsync(id));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfileResponse>>> GetAll()
    {
        return Ok(await _profileService.GetAllProfilesAsync());
    }

    [HttpGet("update")]
    public async Task<ActionResult<ProfileResponse>> Update([FromForm]ProfileUpdateRequest profileUpdateRequest)
    {
        var result = await _profileService.UpdateAsync(profileUpdateRequest);

        var userUpdated = _mapper.Map<UserUpdated>(result);

        await _publishEndpoint.Publish(userUpdated);
        
        return Ok(result);
    }

    [HttpGet("delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute]string id)
    {
        await _profileService.DeleteAsync(id);

        var userDeleted = new UserDeleted()
        {
            Id = id
        };

        await _publishEndpoint.Publish(userDeleted);
        
        return Ok();
    }
}