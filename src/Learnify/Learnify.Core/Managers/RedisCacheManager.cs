using System.Text.Json;
using Learnify.Core.ManagerContracts;
using Microsoft.Extensions.Caching.Distributed;

namespace Learnify.Core.Managers;

public class RedisCacheManager : IRedisCacheManager
{
    private readonly IDistributedCache _cache;

    public RedisCacheManager(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T> GetCachedDataAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var jsonData = await _cache.GetStringAsync(key, cancellationToken);

        if (jsonData == null)
            return default;

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public async Task SetCachedDataAsync<T>(string key, T data, TimeSpan cacheDuration,
        CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = cacheDuration
        };
        var jsonData = JsonSerializer.Serialize(data);
        await _cache.SetStringAsync(key, jsonData, options, cancellationToken);
    }

    public async Task RemoveCachedDataAsync(string key, CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}