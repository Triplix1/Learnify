using IdentityService.Models;
using IdentityService.Repository.Contracts.Base;

namespace IdentityService.Repository.Contracts;

public interface ITemporaryUserRepository : IGetByIdAsync<TemporaryUser, Guid>, ICreatableAsync<TemporaryUser>, IDeletableAsync<Guid>
{
    
}