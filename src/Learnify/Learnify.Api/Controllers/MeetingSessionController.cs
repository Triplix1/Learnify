using Learnify.Api.Controllers.Base;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class MeetingSessionController : BaseApiController
{
    private readonly IMeetingSessionService _meetingSessionService;

    public MeetingSessionController(IMeetingSessionService meetingSessionService)
    {
        _meetingSessionService = meetingSessionService;
    }

    [HttpGet("{courseId}")]
    public async Task<ActionResult<string>> GetMeetingSessionForCourseAsync([FromRoute]int courseId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        
        var result =
            await _meetingSessionService.GetMeetingSessionForCourseAsync(courseId, userId, cancellationToken: cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("{courseId}")]
    public async Task<ActionResult<string>> GetOrCreateMeetingSessionAsync([FromRoute]int courseId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        
        var result =
            await _meetingSessionService.GetOrCreateSessionAsync(courseId, userId, cancellationToken: cancellationToken);

        return Ok(result);
    }
    
    [HttpPost("stop/{sessionId}")]
    public async Task<ActionResult> StopSessionAsync([FromRoute]string sessionId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();
        await _meetingSessionService.StopSessionAsync(sessionId, userId, cancellationToken: cancellationToken);

        return Ok();
    }
}