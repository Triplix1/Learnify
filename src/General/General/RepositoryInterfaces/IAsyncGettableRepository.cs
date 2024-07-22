namespace General.RepositoryInterfaces;

/// <summary>
/// Repository for getting records from storage
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IAsyncGettableRepository<T>
{
    /// <summary>
    /// Returns all objects
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();
}