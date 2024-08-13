using System.Linq.Expressions;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts.Base;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories.Base;

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
    public async Task<PagedList<T>> GetFilteredAsync(MongoFilter<T> filter)
    {
        var query = _collection.Find(Builders<T>.Filter.Where(filter.Specification.GetExpression()));

        var count = await query.CountDocumentsAsync();
        var items = await query.Skip((filter.PageNumber - 1) * filter.PageSize)
            .Limit(filter.PageSize)
            .ToListAsync();

        return new PagedList<T>(items, (int)count, 1, filter.PageSize);
    }

    /// <inheritdoc />
    public async Task<T> UpdateAsync<TField>(TKey key, params (Expression<Func<T, TField>> Field, TField Value)[] updatedFields)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, key);

        if (updatedFields is null || updatedFields.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(updatedFields),
                $"{nameof(updatedFields)} should be more than 1");

        UpdateDefinition<T> update = null;

        foreach (var updatedField in updatedFields)
        {
            if (update is null)
            {
                update = Builders<T>.Update.Set(updatedField.Field, updatedField.Value);
            }
            else
            {
                update = update.Set(updatedField.Field, updatedField.Value);
            }
        }

        await _collection.UpdateOneAsync(filter, update);

        return await _collection.Find(filter).FirstOrDefaultAsync();
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