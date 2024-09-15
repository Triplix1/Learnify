using Learnify.Core.Domain.Entities.Sql;

namespace Learnify.Core.Domain.RepositoryContracts;

public interface IGroupRepository
{
    Task<Group> GetByNameAsync(string name);
    Task<Group> CreateAsync(Group group);
    Task<Group> UpdateAsync(Group group);
    Task<Group> GetByConnectionIdAsync(string connectionId);
}