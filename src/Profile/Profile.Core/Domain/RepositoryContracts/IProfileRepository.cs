using General.RepositoryInterfaces;
using Profile.Core.Domain.Entities;

namespace Profile.Core.Domain.RepositoryContracts;

/// <summary>
/// Repository for working with db entity of profile
/// </summary>
public interface IProfileRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(string key);
    Task<User> CreateAsync(User entity);
    Task<User> UpdateAsync(User entity);
    Task<bool> DeleteAsync(User entity);
}