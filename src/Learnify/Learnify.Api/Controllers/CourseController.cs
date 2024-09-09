using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class CourseController: BaseApiController
{
    private ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedList<CourseTitleResponse>>>> GetCourseTitlesAsync()
    {
        var result = await _courseService.GetAllCourseTitles();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> GetCourseAsync(int id)
    {
        var courseResponse = await _courseService.GetByIdAsync(id);

        return courseResponse;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> CreateCourseAsync([FromBody]CourseCreateRequest courseCreateRequest)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.CreateAsync(courseCreateRequest, userId);

        return courseResponse;
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> PublishCourseAsync([FromRoute]int id, [FromBody]bool publish)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.PublishAsync(id, publish, userId);

        return courseResponse;
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<ApiResponse<CourseResponse>>> UpdateCourse([FromBody]CourseUpdateRequest courseUpdateRequest)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.UpdateAsync(courseUpdateRequest, userId);

        return courseResponse;
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteAsync([FromRoute]int id)
    {
        var userId = User.GetUserId();
        var response = await _courseService.DeleteAsync(id, userId);

        return response;
    }
}