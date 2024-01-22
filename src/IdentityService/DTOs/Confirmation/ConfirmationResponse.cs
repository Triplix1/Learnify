namespace IdentityService.DTOs;

public class ConfirmationResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
}