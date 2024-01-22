namespace IdentityService.Repository.Contracts.Base;

public interface IDeletableAsync<TKey>
{
    public Task<bool> DeleteAsync(TKey id);
}