using Learnify.Core.Domain.Entities;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// base repository for each entity
/// </summary>
/// <typeparam name="T"><see cref="BaseEntity"/></typeparam>
public interface IBaseRepository<T> where T: BaseEntity
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
    Task<T?> GetByIdAsync(int key);
    
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
    Task<bool> DeleteAsync(int id);
    
    /// <summary>
    /// Save changes made on db context
    /// </summary>
    /// <returns></returns>
    Task SaveChangesAsync();
}