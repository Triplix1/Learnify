using Learnify.Api.Controllers.Base;
using Learnify.Core.Attributes;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers;

public class MeetingController: BaseApiController
{
    private readonly IMeetingWebhookService _meetingWebhookService;

    public MeetingController(IMeetingWebhookService meetingWebhookService)
    {
        _meetingWebhookService = meetingWebhookService;
    }

    // ngrok http --url=special-porpoise-stirring.ngrok-free.app 5000
    [SkipApiResponse]
    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook(CancellationToken cancellationToken = default)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);

        await _meetingWebhookService.HandleRequestAsync(json);
        
        return Ok();
    }
}