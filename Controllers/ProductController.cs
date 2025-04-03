using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace RedisCacheDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMemoryCache _memoryCache;
    private readonly string cacheKey = "product_1";

    public ProductController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    [HttpGet("get-product-absolute")]
    public IActionResult GetProductWithAbsoluteExpiration()
    {
        if (_memoryCache.TryGetValue(cacheKey, out string product))
        {
            return Ok(new { source = "cache", data = product });
        }

        product = "Laptop - Dell XPS 15";

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

        var cacheEntry = _memoryCache.CreateEntry(cacheKey);
        cacheEntry.Value = product;
        cacheEntry.SetOptions(cacheOptions);

        cacheEntry.RegisterPostEvictionCallback((key, value, reason, state) =>
        {
            Console.WriteLine($"Cache silindi! Anahtar: {key}, Sebep: {reason}");
        });

        return Ok(new { source = "database", data = product });
    }

    [HttpGet("get-product-sliding")]
    public IActionResult GetProductWithSlidingExpiration()
    {
        if (_memoryCache.TryGetValue(cacheKey, out string product))
        {
            return Ok(new { source = "cache", data = product });
        }

        product = "Laptop - Dell XPS 15";

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(60));
        _memoryCache.Set(cacheKey, product, cacheOptions);

        return Ok(new { source = "database", data = product });
    }

    [HttpGet("remove-product-cache")]
    public IActionResult RemoveProductCache()
    {
        _memoryCache.Remove(cacheKey);
        return Ok("Cache temizlendi.");
    }
}