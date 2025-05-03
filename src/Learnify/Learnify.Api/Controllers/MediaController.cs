using Learnify.Api.Controllers.Base;
using Learnify.Core.Attributes;
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

        if(fileStreamResponse.Protected)
            Response.Headers.Append("Content-Disposition", "inline");

        return File(fileStreamResponse.Stream, fileStreamResponse.ContentType, true);
    }
    
    // [HttpGet("video/{fileId}")]
    // public async Task<ActionResult<UrlResponse>> GetProtectedVideoFile(int fileId, CancellationToken cancellationToken = default)
    // {
    //     var userId = HttpContext.User.GetUserId();
    //
    //     var url = await _fileService.GetHlsManifestUrl(fileId, userId, cancellationToken);
    //
    //     return Ok(url);
    // }

    // public async Task<IActionResult> GetFileAsync(int fileId, CancellationToken cancellationToken = default)
    // {
    //     var userId = HttpContext.User.GetUserId();
    //
    //     var fileStreamResponse = await _fileService.GetFileStreamById(fileId, userId, cancellationToken);
    //
    //     if(fileStreamResponse.Protected)
    //         Response.Headers.Append("Content-Disposition", "inline");
    //
    //     return File(fileStreamResponse.Stream, fileStreamResponse.ContentType, true);
    // }

    [Authorize]
    [HttpPost]
    [RequestSizeLimit((long)10 * 1024 * 1024 * 1024)]
    public async Task<ActionResult<PrivateFileDataResponse>> CreateAsync(
        [FromForm]PrivateFileBlobCreateRequest fileBlobCreateRequest, bool isPublic = false, CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.GetUserId();

        var fileResponse = await _fileService.CreateAsync(fileBlobCreateRequest, isPublic, userId, cancellationToken);

        return Ok(fileResponse);
    }
}