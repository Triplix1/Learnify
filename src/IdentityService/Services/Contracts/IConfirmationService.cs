using IdentityService.DTOs;

namespace IdentityService.Services.Contracts;

public interface IConfirmationService
{
    public Task<ConfirmationResponse> CreateAsync(ConfirmationAddRequest confirmationAddRequest);
    public Task DeleteAsync(Guid id);
    public Task<ConfirmationResponse?> GetByEmailAsync(string email);
}