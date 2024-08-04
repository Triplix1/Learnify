using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data.Interfaces;
using MongoDB.Driver;

namespace Learnify.Infrastructure.Repositories;

/// <summary>
/// Base repository for mongo entities
/// </summary>
/// <typeparam name="T">Type of entity</typeparam>
/// <typeparam name="TKey">Type's key</typeparam>
public class BaseMongoRepository<T, TKey>: IBaseRepository<T, TKey> where T: BaseEntity<TKey>
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
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(_ => true).ToListAsync();
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