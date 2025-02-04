using Learnify.Core.Dto.Payment;
using Stripe;
using StripeResponse = Learnify.Core.Dto.Payment.StripeResponse;

namespace Learnify.Core.ServiceContracts;

public interface IPaymentService
{
    Task<StripeResponse> CheckoutCourseAsync(PaymentCreateRequest paymentCreateRequest, int userId,
        CancellationToken cancellationToken = default);
    
    Task HandleStripeEventAsync(Event stripeEvent, CancellationToken cancellationToken = default);
}