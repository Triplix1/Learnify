using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Payment;
using Learnify.Core.Dto.UserBought;
using Learnify.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using StripeResponse = Learnify.Core.Dto.Payment.StripeResponse;

namespace Learnify.Core.Services;

public class PaymentService : IPaymentService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly StripeOptions _stripeOptions;
    private readonly IUserBoughtService _userBoughtService;
    private const string UserId = "UserId";
    private const string CourseId = "CourseId";

    public PaymentService(IOptions<StripeOptions> stripeSettings, ICourseRepository courseRepository,
        IHttpContextAccessor httpContextAccessor, IUserBoughtService userBoughtService)
    {
        _courseRepository = courseRepository;
        _httpContextAccessor = httpContextAccessor;
        _userBoughtService = userBoughtService;
        _stripeOptions = stripeSettings.Value;

        StripeConfiguration.ApiKey = _stripeOptions.SecretKey;
    }

    public async Task<StripeResponse> CheckoutCourseAsync(PaymentCreateRequest paymentCreateRequest, int userId,
        CancellationToken cancellationToken = default)
    {
        var courseDetails =
            await _courseRepository.GetCoursePaymentDataAsync(paymentCreateRequest.CourseId, cancellationToken);

        var request = _httpContextAccessor.HttpContext!.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";

        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        Currency = "usd",
                        UnitAmount = (int)(courseDetails.Price * 100m),
                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = courseDetails.Name
                        },
                    },
                    Quantity = 1,
                },
            },
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            Metadata = new Dictionary<string, string>
            {
                { UserId, userId.ToString() },
                { CourseId, paymentCreateRequest.CourseId.ToString() }
            },
            Mode = "payment",
            SuccessUrl = paymentCreateRequest.SuccessUrl  + $"/{paymentCreateRequest.CourseId}",
            CancelUrl = paymentCreateRequest.CancelUrl  + $"/{paymentCreateRequest.CourseId}",
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options, cancellationToken: cancellationToken);

        return new StripeResponse
        {
            SessionId = session.Id,
            PublicKey = _stripeOptions.PublicKey
        };
    }

    public async Task HandleStripeEventAsync(Event stripeEvent, CancellationToken cancellationToken = default)
    {
        switch (stripeEvent.Type)
        {
            case EventTypes.CheckoutSessionCompleted:
                await HandleSucceedCheckoutStripeEventAsync(stripeEvent, cancellationToken: cancellationToken);
                break;
        }
    }

    private async Task HandleSucceedCheckoutStripeEventAsync(Event stripeEvent,
        CancellationToken cancellationToken = default)
    {
        var session = stripeEvent.Data.Object as Session;

        if (session is null)
            throw new Exception("Cannot get session");

        var courseBoughtCreateRequest = new UserBoughtCreateRequest()
        {
            UserId = int.Parse(session.Metadata[UserId]),
            CourseId = int.Parse(session.Metadata[CourseId])
        };

        await _userBoughtService.SaveSucceedCourseBoughtResultAsync(courseBoughtCreateRequest,
            cancellationToken: cancellationToken);
    }
}