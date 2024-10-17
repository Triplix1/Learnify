using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.File;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

[Route("api/media")]
public class MediaController: BaseApiController
{
    private readonly IFileService _fileService;

    public MediaController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetStreamForFileAsync(int fileId)
    {
        var userId = HttpContext.User.GetUserId();

        var fileStreamResponse = await _fileService.GetFileStreamById(fileId, userId);

        return File(fileStreamResponse.Stream, fileStreamResponse.ContentType, enableRangeProcessing: true);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<PrivateFileDataResponse>>> CreateAsync([FromForm]PrivateFileBlobCreateRequest fileBlobCreateRequest)
    {
        var userId = HttpContext.User.GetUserId();

        var fileResponse = await _fileService.CreateAsync(fileBlobCreateRequest, userId);

        return Ok(fileResponse);
    }
}