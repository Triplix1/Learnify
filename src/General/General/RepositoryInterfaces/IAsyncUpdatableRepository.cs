namespace General.RepositoryInterfaces;

/// <summary>
/// Updates all instances of 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncUpdatableRepository<T>
{
    /// <summary>
    /// Updates object in storage
    /// </summary>
    /// <param name="entity">object to update</param>
    /// <returns></returns>
    Task<T?> UpdateAsync(T entity);
}