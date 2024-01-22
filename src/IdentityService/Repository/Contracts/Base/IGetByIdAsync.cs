namespace IdentityService.Repository.Contracts.Base;

public interface IGetByIdAsync<T, TKey>
{
    public Task<T> GetByIdAsync(TKey id);
}