using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Specification;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Base repository for mongo entities
/// </summary>
/// <typeparam name="T">Type of entity</typeparam>
/// <typeparam name="TKey">Type's key</typeparam>
public class BaseMongoRepository<T, TKey>: IBaseMongoRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// <see cref="IMongoAppDbContext"/>
    /// </summary>
    protected IMongoAppDbContext Context { get; }
    private readonly IMongoCollection<T> _collection;

    /// <summary>
    /// Initializes a new instance of <see cref="BaseMongoRepository{T,TKey}"/>
    /// </summary>
    /// <param name="context"><see cref="IMongoAppDbContext"/></param>
    /// <param name="collectionName">Collection name</param>
    protected BaseMongoRepository(IMongoAppDbContext context, string collectionName)
    {
        Context = context;
        _collection = context.GetCollection<T>(collectionName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetFilteredAsync(MongoFilter<T> filter)
    {
        var result = _collection.AsQueryable();

        if (filter.Specification is not null)
            result = (IMongoQueryable<T>)Queryable.Where(result, filter.Specification.GetExpression());

        if (filter.Pagination is not null)
            result = (IMongoQueryable<T>)Queryable.Take(result.Skip(filter.Pagination.Skip), filter.Pagination.Take);
        
        return result.ToArray();
    }

    /// <inheritdoc />
    public async Task<T?> GetByIdAsync(TKey key)
    {
        return await _collection.Find(x => x.Id.Equals(key)).FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<T> CreateAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
        
        return entity;
    }

    /// <inheritdoc />
    public async Task<T> UpdateAsync(T entity)
    {
        var result = await _collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity);
        if (result.IsAcknowledged)
        {
            return entity;
        }
        throw new Exception("Update failed");
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(TKey id)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id.Equals(id));
        return result.DeletedCount > 0;
    }
}