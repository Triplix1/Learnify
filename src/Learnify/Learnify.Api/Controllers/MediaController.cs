﻿using Learnify.Api.Controllers.Base;
using Learnify.Core.Attributes;
using Learnify.Core.Dto;
using Learnify.Core.Dto.File;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

[Route("api/media")]
public class MediaController : BaseApiController
{
    private readonly IFileService _fileService;

    public MediaController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [SkipApiResponse]
    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetStreamForFileAsync(int fileId, CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.GetUserId();

        var fileStreamResponse = await _fileService.GetFileStreamById(fileId, userId, cancellationToken);

        return File(fileStreamResponse.Stream, fileStreamResponse.ContentType, true);
    }

    [Authorize]
    [HttpPost]
    [RequestSizeLimit((long)10 * 1024 * 1024 * 1024)]
    public async Task<ActionResult<PrivateFileDataResponse>> CreateAsync(
        [FromForm]PrivateFileBlobCreateRequest fileBlobCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.GetUserId();

        var fileResponse = await _fileService.CreateAsync(fileBlobCreateRequest, userId, cancellationToken);

        return Ok(fileResponse);
    }
}