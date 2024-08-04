using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public abstract class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// Gets or sets a new instance of Context
    /// </summary>
    protected readonly ApplicationDbContext Context;

    /// <summary>
    /// Initializes a new instance of <see cref="BaseRepository{T, Tkey}"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(TKey key)
    {
        return await Context.Set<T>().FindAsync(key);
    }

    /// <inheritdoc />
    public virtual async Task<T> CreateAsync(T entity)
    {
        return (await Context.Set<T>().AddAsync(entity)).Entity;
    }

    /// <inheritdoc />
    public virtual async Task<T> UpdateAsync(T entity)
    {
        Context.Attach(entity);

        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        var entity = await Context.Set<T>().FindAsync(id);

        if (entity is null)
        {
            return false;
        }
        
        Context.Set<T>().Remove(entity);

        return await Context.SaveChangesAsync() > 0;
    }
}