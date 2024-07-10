namespace General.RepositoryInterfaces;

public interface IAsyncGettableByIdRepository<T, TKey>
{
    Task<T?> GetByIdAsync(TKey id);
}