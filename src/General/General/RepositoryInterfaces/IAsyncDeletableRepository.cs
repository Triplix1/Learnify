namespace General.RepositoryInterfaces;

public interface IAsyncDeletableRepository<TKey>
{
    Task<bool> DeleteAsync(TKey id);
}