using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course.LessonDtos;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class LessonController: BaseApiController
{
    private readonly ILessonService _lessonService;

    public LessonController(ILessonService lessonService)
    {
        _lessonService = lessonService;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<LessonResponse>>> GetLessonByIdAsync(string id)
    {
        var userId = User.GetUserId();
        var response = await _lessonService.GetByIdAsync(id, userId);

        return response;
    }

    [HttpGet("paragraph/{paragraphId}")]
    public async Task<ApiResponse<IEnumerable<LessonTitleResponse>>> GetTitlesByParagraphIdAsync(int paragraphId)
    {
        var response = await _lessonService.GetByParagraphAsync(paragraphId);

        return response;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<LessonUpdateResponse>>> UpdateAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest)
    {
        var userId = User.GetUserId();
        
        var response = await _lessonService.AddOrUpdateAsync(lessonAddOrUpdateRequest, userId);

        return response;
    }
    
    [Authorize]
    [HttpPost("draft")]
    public async Task<ActionResult<ApiResponse<LessonUpdateResponse>>> SaveDraftAsync(LessonAddOrUpdateRequest lessonAddOrUpdateRequest)
    {
        var userId = User.GetUserId();
        
        var response = await _lessonService.SaveDraftAsync(lessonAddOrUpdateRequest, userId);

        return response;
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteAsync(string id)
    {
        var userId = User.GetUserId();
        
        var response = await _lessonService.DeleteAsync(id, userId);

        return response;
    }
}