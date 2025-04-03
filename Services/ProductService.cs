using Microsoft.Extensions.Caching.Memory;
using RedisCacheDemo.Models;

namespace RedisCacheDemo.Services;
public class ProductService
{
    private readonly IMemoryCache _memoryCache;

    public ProductService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public void CacheProduct(Product product)
    {
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        _memoryCache.Set($"product_{product.Id}", product, cacheOptions);
    }

    public Product GetCachedProduct(int productId)
    {
        if (_memoryCache.TryGetValue($"product_{productId}", out Product cachedProduct))
        {
            return cachedProduct;
        }

        return null;
    }
}