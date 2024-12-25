using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class CourseController : BaseApiController
{
    private ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<CourseTitleResponse>>> GetCourseTitlesAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await _courseService.GetAllCourseTitles(cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CourseResponse>> GetCourseAsync(int id,
        CancellationToken cancellationToken = default)
    {
        var courseResponse = await _courseService.GetByIdAsync(id, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CourseResponse>> CreateCourseAsync(
        [FromBody]CourseCreateRequest courseCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.CreateAsync(courseCreateRequest, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<CourseResponse>> PublishCourseAsync([FromRoute]int id,
        [FromBody]bool publish, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.PublishAsync(id, publish, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult<CourseResponse>> UpdateCourse(
        [FromBody]CourseUpdateRequest courseUpdateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.UpdateAsync(courseUpdateRequest, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync([FromRoute]int id,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        await _courseService.DeleteAsync(id, userId, cancellationToken);

        return Ok();
    }
}