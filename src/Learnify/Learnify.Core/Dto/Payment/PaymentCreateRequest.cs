namespace Learnify.Core.Dto.Payment;

public class PaymentCreateRequest
{
    public int CourseId { get; set; }
    public string SuccessUrl { get; set; }
    public string CancelUrl { get; set; }
}