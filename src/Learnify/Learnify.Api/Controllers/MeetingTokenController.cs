using Learnify.Api.Controllers.Base;
using Learnify.Core.Dto.MeetingToken;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class MeetingTokenController : BaseApiController
{
    private readonly IMeetingTokenService _meetingTokenService;

    public MeetingTokenController(IMeetingTokenService meetingTokenService)
    {
        _meetingTokenService = meetingTokenService;
    }

    [HttpPost("{sessionId}")]
    public async Task<ActionResult<MeetingTokenResponse>> GenerateMeetingToken(string sessionId,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var token = await _meetingTokenService.GenerateMeetingTokenAsync(sessionId, userId, cancellationToken);

        return token;
    }
}