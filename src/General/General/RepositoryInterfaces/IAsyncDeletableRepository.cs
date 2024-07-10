namespace General.RepositoryInterfaces;

public interface IAsyncDeletableRepository<T>
{
    Task<bool> DeleteAsync(T entity);
}