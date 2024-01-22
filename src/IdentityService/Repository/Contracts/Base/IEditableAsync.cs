namespace IdentityService.Repository.Contracts.Base;

public interface IEditableAsync<T>
{
    public Task<T> UpdateAsync(T entity);
}