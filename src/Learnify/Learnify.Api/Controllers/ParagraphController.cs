﻿using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.ParagraphDtos;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class ParagraphController : BaseApiController
{
    private readonly IParagraphService _paragraphService;

    public ParagraphController(IParagraphService paragraphService)
    {
        _paragraphService = paragraphService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ParagraphResponse>> CreateAsync(
        ParagraphCreateRequest paragraphCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var result = await _paragraphService.CreateAsync(paragraphCreateRequest, userId, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ParagraphResponse>> UpdateAsync(
        ParagraphUpdateRequest paragraphUpdateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var result = await _paragraphService.UpdateAsync(paragraphUpdateRequest, userId, cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        await _paragraphService.DeleteAsync(id, userId, cancellationToken);

        return Ok();
    }
}