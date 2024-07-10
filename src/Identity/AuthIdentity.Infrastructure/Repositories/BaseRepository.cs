using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace AuthIdentity.Infrastructure.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T: BaseEntity
{
    protected readonly IdentityDbContext Context;

    protected BaseRepository(IdentityDbContext context)
    {
        Context = context;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await Context.Set<T>().ToListAsync();

    }

    public virtual async Task<T?> GetByIdAsync(Guid key)
    {
        return await Context.Set<T>().FirstOrDefaultAsync(e => e.Id == key);
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        return (await Context.Set<T>().AddAsync(entity)).Entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
        Context.Attach(entity);

        Context.Entry(entity).State = EntityState.Modified;

        await Context.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await Context.Set<T>().FirstOrDefaultAsync(rt => rt.Id == id);

        if (entity is null)
        {
            return false;
        }
        
        Context.Set<T>().Remove(entity);

        return await Context.SaveChangesAsync() > 0;
    }
}