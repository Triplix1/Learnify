namespace General.RepositoryInterfaces;

/// <summary>
/// Repository for getting records from storage
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IAsyncGettableByIdRepository<T, TKey>
{
    /// <summary>
    /// Returns object wit the same Id
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(TKey id);
}