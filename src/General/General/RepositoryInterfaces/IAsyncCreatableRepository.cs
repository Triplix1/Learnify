namespace General.RepositoryInterfaces;

public interface IAsyncCreatableRepository<T>
{
    Task CreateAsync(T entity);
}