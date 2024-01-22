using IdentityService.Models;
using IdentityService.Repository.Contracts.Base;

namespace IdentityService.Repository.Contracts;

public interface IConfirmationRepository: IDeletableAsync<Guid>, ICreatableAsync<Confirmation>
{
    public Task<Confirmation?> GetByEmailAsync(string email);
}