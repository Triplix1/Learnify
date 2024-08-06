using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// UserRepository
/// </summary>
public interface IUserRepository : IBasePsqRepository<User, int>
{
    /// <summary>
    /// Returns user by email
    /// </summary>
    /// <param name="email">Email</param>
    /// <returns></returns>
    Task<User?> GetByEmailAsync(string email);
    /// <summary>
    /// Returns user by username
    /// </summary>
    /// <param name="username">Username</param>
    /// <returns></returns>
    Task<User?> GetByUsernameAsync(string username);
}