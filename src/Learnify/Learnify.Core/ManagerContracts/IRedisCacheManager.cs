namespace Learnify.Core.ManagerContracts;

public interface IRedisCacheManager
{
    Task<T> GetCachedDataAsync<T>(string key, CancellationToken cancellationToken = default);

    Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration,
        CancellationToken cancellationToken = default);

    Task RemoveCachedDataAsync(string key, CancellationToken cancellationToken = default);
}