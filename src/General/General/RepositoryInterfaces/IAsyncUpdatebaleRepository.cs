namespace General.RepositoryInterfaces;

public interface IAsyncUpdatebaleRepository<T>
{
    Task<T?> UpdateAsync(T entity);
}