using Learnify.Core.Domain.Entities;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts.Base;

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
    Task<PagedList<T>> GetFilteredAsync(EfFilter<T> efFilter, CancellationToken cancellationToken = default);
}