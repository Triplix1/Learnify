namespace Learnify.Core.Dto.Payment;

public class StripeResponse
{
    public string SessionId { get; set; }
    public string PublicKey { get; set; }
}