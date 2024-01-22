using IdentityService.DTOs.TemporaryUser;

namespace IdentityService.Services.Contracts;

public interface ITemporaryUserService
{
    public Task<TemporaryUserResponse> CreateAsync(TemporaryUserAddRequest temporaryUserAddRequest);
    public Task DeleteAsync(Guid id);
    public Task<TemporaryUserResponse?> GetByIdAsync(Guid id);
}