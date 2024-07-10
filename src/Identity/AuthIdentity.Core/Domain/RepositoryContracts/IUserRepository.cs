using AuthIdentity.Core.Domain.Entities;

namespace AuthIdentity.Core.Domain.RepositoryContracts;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string username);
}