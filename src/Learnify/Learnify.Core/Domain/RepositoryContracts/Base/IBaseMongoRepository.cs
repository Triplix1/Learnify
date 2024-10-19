using System.Linq.Expressions;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;

namespace Learnify.Core.Domain.RepositoryContracts.Base;

// /// <summary>
// /// Base repository for mongo entities
// /// </summary>
// public interface IBaseMongoRepository<T, TKey>: IBaseRepository<T, TKey> where T: BaseEntity<TKey>
// {
//     /// <summary>
//     /// Returns all entities
//     /// </summary>
//     /// <returns></returns>
//     Task<PagedList<T>> GetFilteredAsync(MongoFilter<T> filter, CancellationToken cancellationToken = default);
//
//     /// <summary>
//     /// Updates only specific
//     /// </summary>
//     /// <param name="key"></param>
//     /// <param name="field"></param>
//     /// <param name="value"></param>
//     /// <typeparam name="TField"></typeparam>
//     /// <returns></returns>
//     Task<T> UpdateAsync<TField>(TKey key, params (Expression<Func<T, TField>> Field, TField Value)[] updatedFields);
// }