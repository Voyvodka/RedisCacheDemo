using Microsoft.AspNetCore.Mvc;
using RedisCacheDemo.Services;

namespace RedisCacheDemo.Area.Demo1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IRedisService _redisService;
    private readonly IRedisCacheService _redisCacheService;

    public CartController(IRedisService redisService, IRedisCacheService redisCacheService)
    {
        _redisService = redisService;
        _redisCacheService = redisCacheService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(string userId)
    {
        // var cartItems = await _redisService.GetCartAsync(userId);
        var cartItemsCache = await _redisCacheService.GetCacheAsync(userId);
        return Ok(cartItemsCache);
    }

    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddToCart(string userId, string productId)
    {
        await _redisService.AddToCartAsync(userId, productId);
        await _redisCacheService.SetCacheAsync(userId, productId);
        return Ok("Ürün sepete eklendi.");
    }
}
