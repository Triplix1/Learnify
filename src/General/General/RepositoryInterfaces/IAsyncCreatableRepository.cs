namespace General.RepositoryInterfaces;

/// <summary>
/// Repository for creating
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncCreatableRepository<T>
{
    /// <summary>
    /// Saves specified instance
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task CreateAsync(T entity);
}