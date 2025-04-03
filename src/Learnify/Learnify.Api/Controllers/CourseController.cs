using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto;
using Learnify.Core.Dto.Course;
using Learnify.Core.Dto.File;
using Learnify.Core.Dto.Params;
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
    public async Task<ActionResult<PagedList<CourseTitleResponse>>> GetCourseTitlesAsync([FromQuery]CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var result = await _courseService.GetAllCourseTitles(courseParams, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("my-courses")]
    public async Task<ActionResult<PagedList<CourseTitleResponse>>> GetMyCourseTitlesAsync([FromQuery]CourseParams courseParams,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var result = await _courseService.GetMyCourseTitles(userId, courseParams, cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("main-info/{id}")]
    public async Task<ActionResult<PagedList<CourseMainInfoResponse>>> GetMainCourseInfoAsync([FromRoute]int id,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var result = await _courseService.GetMainInfoResponseAsync(id, userId, cancellationToken);

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
    [HttpGet("study/{id}")]
    public async Task<ActionResult<CourseStudyResponse>> GetCourseToStudyAsync([FromRoute]int id, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.GetCourseStudyResponseAsync(id, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CourseUpdateResponse>> CreateCourseAsync(
        [FromBody]CourseCreateRequest courseCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.CreateAsync(courseCreateRequest, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<CourseUpdateResponse>> PublishCourseAsync([FromRoute]int id,
        [FromBody]PublishCourseRequest publish, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var courseResponse = await _courseService.PublishAsync(id, publish.Publish, userId, cancellationToken);

        return Ok(courseResponse);
    }

    [RequestSizeLimit((long)10 * 1024 * 1024 * 1024)]
    [HttpPost("photo")]
    public async Task<ActionResult<PrivateFileDataResponse>> SaveCoursePhotoAsync(
        [FromForm]PrivateFileBlobCreateRequest fileBlobCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var response =
            await _courseService.UpdatePhotoAsync(userId, fileBlobCreateRequest, cancellationToken: cancellationToken);
        
        return Ok(response);
    }
    
    [RequestSizeLimit((long)10 * 1024 * 1024 * 1024)]
    [HttpPost("video")]
    public async Task<ActionResult<PrivateFileDataResponse>> SaveCourseVideoAsync(
        [FromForm]PrivateFileBlobCreateRequest fileBlobCreateRequest, CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        var response =
            await _courseService.UpdateVideoAsync(userId, fileBlobCreateRequest, cancellationToken: cancellationToken);
        
        return Ok(response);
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