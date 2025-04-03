namespace RedisCacheDemo.Services;

/// <summary>
/// Redis işlemleri ile ilgili sepet ve cache yönetim işlemlerini sağlayan servis.
/// </summary>
public interface IRedisService
{
    /// <summary>
    /// Kullanıcının sepetine ürün ekler.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="productId">Ürün ID'si</param>
    Task AddToCartAsync(string userId, string productId);

    /// <summary>
    /// Kullanıcının sepetindeki tüm ürünleri getirir.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <returns>Sepetteki ürünlerin listesi</returns>
    Task<List<string>> GetCartAsync(string userId);

    /// <summary>
    /// Kullanıcının sepetinden bir ürünü siler.
    /// </summary>
    /// <param name="userId">Kullanıcı ID'si</param>
    /// <param name="productId">Silinecek ürünün ID'si</param>
    Task RemoveFromCartAsync(string userId, string productId);

    /// <summary>
    /// Ürün bilgilerini cache'e ekler.
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <param name="productDetails">Ürün detayları</param>
    Task SetProductCacheAsync(string productId, string productDetails);

    /// <summary>
    /// Ürün bilgilerini cache'den getirir.
    /// </summary>
    /// <param name="productId">Ürün ID'si</param>
    /// <returns>Ürün bilgisi</returns>
    Task<string> GetProductCacheAsync(string productId);

    /// <summary>
    /// Kullanıcı session bilgisini Redis'e kaydeder.
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <param name="userData">Kullanıcı verisi</param>
    Task SetSessionAsync(string sessionId, string userData);

    /// <summary>
    /// Kullanıcı session bilgisini Redis'ten getirir.
    /// </summary>
    /// <param name="sessionId">Session ID</param>
    /// <returns>Kullanıcı verisi</returns>
    Task<string> GetSessionAsync(string sessionId);
}
