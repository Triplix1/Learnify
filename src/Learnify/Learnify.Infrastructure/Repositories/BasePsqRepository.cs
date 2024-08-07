﻿using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Specification;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories;

/// <inheritdoc />
public abstract class BasePsqRepository<T, TKey> : IBasePsqRepository<T, TKey> where T: BaseEntity<TKey>
{
    /// <summary>
    /// Gets or sets a new instance of Context
    /// </summary>
    protected readonly ApplicationDbContext Context;

    /// <summary>
    /// Initializes a new instance of <see cref="BasePsqRepository{T,TKey}"/>
    /// </summary>
    /// <param name="context"><see cref="ApplicationDbContext"/></param>
    protected BasePsqRepository(ApplicationDbContext context)
    {
        Context = context;
    }

    /// <inheritdoc />
    public virtual async Task<IEnumerable<T>> GetFilteredAsync(EfFilter<T> efFilter)
    {
        var result = Context.Set<T>().AsQueryable();
        
        if(efFilter.Includes is not null && efFilter.Includes.Count != 0)
            result = efFilter.Includes.Aggregate(
                result,
                (current, include) => current.Include(include)
            );

        if (efFilter.Specification is not null)
            result = result.Where(efFilter.Specification.GetExpression());

        if (efFilter.Pagination is not null)
            result = result.Skip(efFilter.Pagination.Skip)
                .Take(efFilter.Pagination.Take);
        
        return await result.ToArrayAsync();
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