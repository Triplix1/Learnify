using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class LessonController : BaseApiController
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LessonResponse>>> GetLessonByIdAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var response = await _lessonService.GetByIdAsync(id, userId, cancellationToken);

        return response;
    }

    [Authorize]
    [HttpGet("for-update/{id}")]
    public async Task<ActionResult<ApiResponse<LessonUpdateResponse>>> GetLessonForUpdateAsync(string id,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var response = await _lessonService.GetForUpdateAsync(id, userId, cancellationToken);

        return response;
    }

    [HttpGet("titles/{paragraphId}")]
    public async Task<ApiResponse<IEnumerable<LessonTitleResponse>>> GetTitlesByParagraphIdAsync(int paragraphId,
        [FromQuery]bool includeDraft = false, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _lessonService.GetByParagraphAsync(paragraphId, userId, includeDraft, cancellationToken);

        return response;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LessonUpdateResponse>>> AddOrUpdateAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _lessonService.AddOrUpdateAsync(lessonAddOrUpdateRequest, userId, cancellationToken);

        return response;
    }

    [Authorize]
    [HttpPost("draft")]
    public async Task<ActionResult<ApiResponse<LessonUpdateResponse>>> SaveDraftAsync(
        LessonAddOrUpdateRequest lessonAddOrUpdateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _lessonService.SaveDraftAsync(lessonAddOrUpdateRequest, userId, cancellationToken);

        return response;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _lessonService.DeleteAsync(id, userId, cancellationToken);

        return response;
    }
}