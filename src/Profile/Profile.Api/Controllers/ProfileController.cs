﻿using AutoMapper;
using Contracts;
using General.Dto;
using General.Etensions;
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
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> GetById([FromRoute]string id)
    {
        var profile = await _profileService.GetByIdAsync(id);
        return Ok(profile.ToApiResponse());
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProfileResponse>>>> GetAll()
    {
        var profiles = await _profileService.GetAllProfilesAsync();
        return Ok(profiles.ToApiResponse());
    }

    [HttpPut("update")]
    public async Task<ActionResult<ApiResponse<ProfileResponse>>> Update([FromForm]ProfileUpdateRequest profileUpdateRequest)
    {
        var result = await _profileService.UpdateAsync(profileUpdateRequest);

        var userUpdated = _mapper.Map<UserUpdated>(result);

        await _publishEndpoint.Publish(userUpdated);
        
        return Ok(result.ToApiResponse());
    }

    [HttpDelete("delete/{id}")]
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