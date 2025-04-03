namespace RedisCacheDemo.Services;

/// <summary>
/// Redis cache işlemleri için temel interface.
/// </summary>
public interface IRedisCacheService
{
    /// <summary>
    /// Cache'e bir string değer ekler.
    /// </summary>
    Task SetCacheAsync(string key, string value, int expirationMinutes = 10);

    /// <summary>
    /// Cache'ten bir string değer okur.
    /// </summary>
    Task<string> GetCacheAsync(string key);

    /// <summary>
    /// Cache'e bir nesne ekler.
    /// </summary>
    Task SetObjectAsync<T>(string key, T value, int expirationMinutes = 10);

    /// <summary>
    /// Cache'ten bir nesne okur.
    /// </summary>
    Task<T?> GetObjectAsync<T>(string key);

    /// <summary>
    /// Cache'ten bir veriyi siler.
    /// </summary>
    Task RemoveCacheAsync(string key);
}