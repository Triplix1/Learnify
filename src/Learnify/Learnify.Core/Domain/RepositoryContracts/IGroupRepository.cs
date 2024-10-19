using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IGroupRepository
{
    Task<Group> GetByNameAsync(string name, IEnumerable<string> includes = null, CancellationToken cancellationToken = default);
    Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default);
    Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default);
    Task<Group> GetByConnectionIdAsync(string connectionId, CancellationToken cancellationToken = default);
    Task<Group> GetByMessageIdAsync(int messageId, CancellationToken cancellationToken = default);
}