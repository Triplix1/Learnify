using Learnify.Api.Controllers.Base;
using Learnify.Core.Attributes;
using Learnify.Core.Dto.Payment;
using Learnify.Core.Enums;
using Learnify.Core.Extensions;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;

namespace Learnify.Api.Controllers;

public class CheckoutController : BaseApiController
{
    private readonly IPaymentService _paymentService;
    private readonly StripeOptions _stripeOptions;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(IPaymentService paymentService, IOptions<StripeOptions> stripeOptions,
        ILogger<CheckoutController> logger)
    {
        _paymentService = paymentService;
        _stripeOptions = stripeOptions.Value;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    [Authorize(Roles = nameof(Role.Student))]
    public async Task<IActionResult> CreateSession([FromBody]PaymentCreateRequest paymentCreateRequest,
        CancellationToken cancellationToken = default)
    {
        var userId = User.GetUserId();

        var response = await _paymentService.CheckoutCourseAsync(paymentCreateRequest, userId, cancellationToken);

        return Ok(response);
    }

    [SkipApiResponse]
    [HttpPost("webhook")]
    public async Task<IActionResult> WebHook(CancellationToken cancellationToken = default)
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync(cancellationToken);

        var stripeEvent = EventUtility.ConstructEvent(
            json,
            Request.Headers["Stripe-Signature"],
            _stripeOptions.WHSecret,
            throwOnApiVersionMismatch: false
        );

        try
        {
            await _paymentService.HandleStripeEventAsync(stripeEvent, cancellationToken: cancellationToken);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return BadRequest();
        }
    }
}