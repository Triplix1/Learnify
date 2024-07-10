namespace General.RepositoryInterfaces;

public interface IAsyncGettableRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync();
}