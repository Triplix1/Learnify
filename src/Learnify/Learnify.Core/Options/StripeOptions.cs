namespace Learnify.Core.Dto.Payment;

public class StripeOptions
{
    public string PublicKey { get; set; }
    public string SecretKey { get; set; }
    public string WHSecret { get; set; }
}