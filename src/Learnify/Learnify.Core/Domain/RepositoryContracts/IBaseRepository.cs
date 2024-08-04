using Learnify.Core.Domain.Entities;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// base repository for each entity
/// </summary>
/// <typeparam name="T"><see cref="BaseEntity"/></typeparam>
/// <typeparam name="TKey">Key</typeparam>
public interface IBaseRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// Returns all entities
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();
    
    /// <summary>
    /// Returns entity by id
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetByIdAsync(TKey key);
    
    /// <summary>
    /// Creates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> CreateAsync(T entity);
    
    /// <summary>
    /// Updates entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<T> UpdateAsync(T entity);
    
    /// <summary>
    /// Deletes entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Success of operation</returns>
    Task<bool> DeleteAsync(TKey id);
}