namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// base interface for Unit of work repositories
/// </summary>
public interface IBaseUnitOfWork
{
    /// <summary>
    /// Saves changes made on db
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}