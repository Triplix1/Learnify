using AutoMapper;
using Contracts.User;
using General.Dto;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Profile.Core.DTO;
using Profile.Core.ServiceContracts;
using ProfileService.Controllers.Base;

namespace ProfileService.Controllers;

/// <summary>
/// Profile controller
/// </summary>
[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IProfileService _profileService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a nwe instance of <see cref="ProfileController"/>
    /// </summary>
    /// <param name="profileService"></param>
    /// <param name="publishEndpoint"></param>
    /// <param name="mapper"></param>
    public ProfileController(IProfileService profileService, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _profileService = profileService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns [rofile with specified Id
    /// </summary>
    /// <param name="id">Profile id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> GetById([FromRoute]string id)
    {
        var profile = await _profileService.GetByIdAsync(id);
        return Ok(profile);
    }

    /// <summary>
    /// Returns all profiles
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProfileResponse>>>> GetAll()
    {
        var profiles = await _profileService.GetAllProfilesAsync();
        return Ok(profiles);
    }

    /// <summary>
    /// Updates profile
    /// </summary>
    /// <param name="profileUpdateRequest"><see cref="ProfileUpdateRequest"/></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> Update([FromForm]ProfileUpdateRequest profileUpdateRequest)
    {
        var result = await _profileService.UpdateAsync(profileUpdateRequest);

        if (result.IsSuccess)
        {
            var userUpdated = _mapper.Map<UserUpdated>(result.Data);

            await _publishEndpoint.Publish(userUpdated);
        }
        
        return Ok(result);
    }

    /// <summary>
    /// Deletes profile with specified id
    /// </summary>
    /// <param name="id">profile's id</param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete([FromRoute]string id)
    {
        var result = await _profileService.DeleteAsync(id);

        if (result.IsSuccess)
        {
            var userDeleted = new UserDeleted()
            {
                Id = id
            };

            await _publishEndpoint.Publish(userDeleted);
        }
        
        return Ok();
    }
}