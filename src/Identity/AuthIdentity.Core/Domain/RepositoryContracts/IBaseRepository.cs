using AuthIdentity.Core.Domain.Entities;

namespace AuthIdentity.Core.Domain.RepositoryContracts;

public interface IBaseRepository<T> where T: BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid key);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task SaveChangesAsync();
}