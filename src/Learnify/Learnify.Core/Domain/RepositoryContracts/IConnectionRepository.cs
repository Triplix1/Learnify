namespace Learnify.Core.Domain.RepositoryContracts;

public interface IConnectionRepository
{
    Task<bool> RemoveAsync(string connectionId);
}