using StackExchange.Redis;

namespace RedisCacheDemo.Services;

/// <summary>
/// Redis üzerinde sepet ve cache yönetimi işlemlerini gerçekleştiren servis.
/// </summary>
public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    /// <summary>
    /// Redis bağlantısı kurulur ve veri tabanı seçilir.
    /// </summary>
    /// <param name="redis">IConnectionMultiplexer bağlantısı</param>
    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    /// <summary>
    /// Kullanıcının sepetine ürün ekler.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="productId">Ürün ID'si</param>
    public async Task AddToCartAsync(string userId, string productId)
    {
        await _db.ListRightPushAsync($"cart:{userId}", productId);
    }

    /// <summary>
    /// Kullanıcının sepetindeki tüm ürünleri getirir.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Sepetteki ürünlerin listesi</returns>
    public async Task<List<string>> GetCartAsync(string userId)
    {
        var cartItems = await _db.ListRangeAsync($"cart:{userId}");
        return cartItems.Select(item => item.ToString()).ToList();
    }

    /// <summary>
    /// Kullanıcının sepetinden bir ürünü siler.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="productId">Silinecek ürünün ID'si</param>
    public async Task RemoveFromCartAsync(string userId, string productId)
    {
        await _db.ListRemoveAsync($"cart:{userId}", productId);
    }

    /// <summary>
    /// Ürün bilgilerini cache'e ekler.
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="productDetails">Ürün detayları</param>
    public async Task SetProductCacheAsync(string productId, string productDetails)
    {
        await _db.StringSetAsync($"product:{productId}", productDetails);
    }

    /// <summary>
    /// Ürün bilgilerini cache'den getirir.
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <returns>Ürün bilgisi</returns>
    public async Task<string> GetProductCacheAsync(string productId)
    {
        return await _db.StringGetAsync($"product:{productId}");
    }

    /// <summary>
    /// Kullanıcı session bilgisini Redis'e kaydeder.
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <param name="userData">Kullanıcı verisi</param>
    public async Task SetSessionAsync(string sessionId, string userData)
    {
        await _db.StringSetAsync($"session:{sessionId}", userData);
    }

    /// <summary>
    /// Kullanıcı session bilgisini Redis'ten getirir.
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <returns>Kullanıcı verisi</returns>
    public async Task<string> GetSessionAsync(string sessionId)
    {
        return await _db.StringGetAsync($"session:{sessionId}");
    }
}
