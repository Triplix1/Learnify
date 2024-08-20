using Learnify.Api.Controllers.Base;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class MediaController: BaseApiController
{
    private readonly IFileService _fileService;

    public MediaController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [Authorize]
    [HttpGet("{fileId}")]
    public async Task<ActionResult<Stream>> GetStreamForFileAsync(int fileId)
    {
        var userId = HttpContext.User.GetUserId();

        var stream = await _fileService.GetFileStreamById(fileId, userId);

        return stream;
    }
}