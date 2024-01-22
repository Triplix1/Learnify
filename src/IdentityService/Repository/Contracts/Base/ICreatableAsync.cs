namespace IdentityService.Repository.Contracts.Base;

public interface ICreatableAsync<T>
{
    public Task<T> CreateAsync(T entity);
}