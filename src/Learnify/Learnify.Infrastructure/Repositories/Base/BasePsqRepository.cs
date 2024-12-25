using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts.Base;
using Learnify.Core.Dto;
using Learnify.Core.Specification.Filters;
using Learnify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Learnify.Infrastructure.Repositories.Base;

/// <inheritdoc />
public abstract class BasePsqRepository<T, TKey> : IBasePsqRepository<T, TKey> where T : BaseEntity<TKey>
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
    public virtual async Task<PagedList<T>> GetFilteredAsync(EfFilter<T> efFilter,
        CancellationToken cancellationToken = default)
    {
        var result = Context.Set<T>().AsQueryable();

        if (efFilter.Includes is not null && efFilter.Includes.Count != 0)
            result = efFilter.Includes.Aggregate(
                result,
                (current, include) => current.Include(include)
            );

        if (efFilter.Specification is not null)
            result = result.Where(efFilter.Specification.GetExpression());

        return await PagedList<T>.CreateAsync(result, efFilter.PageNumber, efFilter.PageSize, cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<T?> GetByIdAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await Context.Set<T>().FindAsync([key], cancellationToken);
    }

    /// <inheritdoc />
    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = (await Context.Set<T>().AddAsync(entity, cancellationToken)).Entity;

        await Context.SaveChangesAsync(cancellationToken);

        return result;
    }

    /// <inheritdoc />
    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        Context.Attach(entity);

        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync(cancellationToken);

        return entity;
    }

    /// <inheritdoc />
    public virtual async Task<bool> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var entity = await Context.Set<T>().FindAsync([id], cancellationToken);

        if (entity is null)
        {
            return false;
        }

        Context.Set<T>().Remove(entity);

        return await Context.SaveChangesAsync(cancellationToken) > 0;
    }
}