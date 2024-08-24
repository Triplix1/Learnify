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
    public async Task<ApiResponse<LessonResponse>> GetLessonById(string id)
    {
        var userId = User.GetUserId();
        var response = await _lessonService.GetByIdAsync(id, userId);

        return response;
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse<LessonResponse>> CreateLesson(LessonCreateRequest lessonCreateRequest)
    {
        var userId = User.GetUserId();
        var response = await _lessonService.CreateAsync(lessonCreateRequest, userId);

        return response;
    }

    [Authorize]
    [HttpPut]
    public async Task<ApiResponse<LessonResponse>> UpdateAsync(LessonUpdateRequest lessonUpdateRequest)
    {
        var userId = User.GetUserId();
        
        var response = await _lessonService.UpdateAsync(lessonUpdateRequest, userId);

        return response;
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DeleteAsync(string id)
    {
        var userId = User.GetUserId();
        
        var response = await _lessonService.DeleteAsync(id, userId);

        return response;
    }
}