﻿using Learnify.Core.Domain.Entities;
using Learnify.Core.Dto;
using Learnify.Core.Specification;

namespace Learnify.Core.Domain.RepositoryContracts;

/// <summary>
/// Base repository for mongo entities
/// </summary>
public interface IBaseMongoRepository<T, TKey>: IBaseRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// Returns all entities
    /// </summary>
    /// <returns></returns>
    Task<PagedList<T>> GetFilteredAsync(MongoFilter<T> filter);
}