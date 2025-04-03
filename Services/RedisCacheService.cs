using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;


namespace RedisCacheDemo.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Cache'e bir string değer ekler.
    /// </summary>
    public async Task SetCacheAsync(string key, string value, int expirationMinutes = 10)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
        };

        await _cache.SetStringAsync(key, value, options);
    }

    /// <summary>
    /// Cache'ten bir string değer okur.
    /// </summary>
    public async Task<string> GetCacheAsync(string key)
    {
        return await _cache.GetStringAsync(key);
    }

    /// <summary>
    /// Cache'e bir nesne ekler.
    /// </summary>
    public async Task SetObjectAsync<T>(string key, T value, int expirationMinutes = 10)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
        };

        var jsonData = JsonSerializer.Serialize(value);
        await _cache.SetStringAsync(key, jsonData, options);
    }

    /// <summary>
    /// Cache'ten bir nesne okur.
    /// </summary>
    public async Task<T?> GetObjectAsync<T>(string key)
    {
        var jsonData = await _cache.GetStringAsync(key);
        return jsonData is not null ? JsonSerializer.Deserialize<T>(jsonData) : default;
    }

    /// <summary>
    /// Cache'ten bir veriyi siler.
    /// </summary>
    public async Task RemoveCacheAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
