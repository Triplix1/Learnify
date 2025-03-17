using Learnify.Core.Attributes;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace Learnify.Api.Controllers.Base;

public class VonageController : BaseApiController
{
    private readonly IMeetingWebhookService _meetingWebhookService;
    private readonly ILogger<VonageController> _logger;

    public VonageController(IMeetingWebhookService meetingWebhookService,
        ILogger<VonageController> logger)
    {
        _meetingWebhookService = meetingWebhookService;
        _logger = logger;
    }

    [SkipApiResponse]
    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook(CancellationToken cancellationToken = default)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);

        try
        {
            await _meetingWebhookService.HandleRequestAsync(json);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest();
        }
    }
}