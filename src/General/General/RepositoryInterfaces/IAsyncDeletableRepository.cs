namespace General.RepositoryInterfaces;

/// <summary>
/// Repository for deleting objects
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncDeletableRepository<T>
{
    /// <summary>
    /// Deletes object from storage
    /// </summary>
    /// <param name="entity">Object to delete</param>
    /// <returns></returns>
    Task<bool> DeleteAsync(T entity);
}