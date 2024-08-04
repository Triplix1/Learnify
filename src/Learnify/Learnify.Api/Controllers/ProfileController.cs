using AutoMapper;
using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
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
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a nwe instance of <see cref="ProfileController"/>
    /// </summary>
    /// <param name="profileService"></param>
    /// <param name="mapper"></param>
    public ProfileController(IProfileService profileService, IMapper mapper)
    {
        _profileService = profileService;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns [rofile with specified Id
    /// </summary>
    /// <param name="id">Profile id</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> GetById([FromRoute]int id)
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
        
        return Ok(result);
    }

    /// <summary>
    /// Deletes profile with specified id
    /// </summary>
    /// <param name="id">profile's id</param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<ApiResponse>> Delete([FromRoute]int id)
    {
        var result = await _profileService.DeleteAsync(id);
        
        return Ok(result);
    }
}