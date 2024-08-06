using Learnify.Core.Domain.Entities;
using Learnify.Core.Specification;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Base repo for postgre entities
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IBasePsqRepository<T, TKey>: IBaseRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// Returns all entities
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetFilteredAsync(EfFilter<T> efFilter);
}