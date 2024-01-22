namespace IdentityService.Repository.Contracts.Base;

public interface IGetAllAsync<T>
{
    public Task<IEnumerable<T>> GetAllAsync();
}