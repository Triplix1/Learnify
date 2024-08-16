namespace Learnify.Core.ManagerContracts;

public interface IRedisCacheManager
{
    Task<T> GetCachedDataAsync<T>(string key);
    Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration);
    Task RemoveCachedDataAsync(string key);
}