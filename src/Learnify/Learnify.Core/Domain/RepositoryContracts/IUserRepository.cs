using Learnify.Core.Domain.Entities.Sql;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IUserRepository
{
    Task<PagedList<User>> GetPagedAsync(EfFilter<User> filter,
        CancellationToken cancellationToken = default);

    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<User> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    Task<User> GetByIdAsync(int id, IEnumerable<string> stringToInclude = default, CancellationToken cancellationToken = default);
    
    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}