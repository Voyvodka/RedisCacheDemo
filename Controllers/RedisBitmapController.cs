using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-bitmap")]
[ApiController]
public class RedisBitmapController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string UserKey = "users:";

    public RedisBitmapController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("set-active/{userId}/{day}")]
    public IActionResult SetUserActive(string userId, int day)
    {
        var key = $"{UserKey}{userId}";
        _redisDb.StringSetBit(key, day, true);

        return Ok($"Kullanıcı {userId}, gün {day} için aktif işaretlendi.");
    }

    [HttpGet("is-active/{userId}/{day}")]
    public IActionResult IsUserActive(string userId, int day)
    {
        var key = $"{UserKey}{userId}";
        bool isActive = _redisDb.StringGetBit(key, day);

        return Ok(isActive ? $"Kullanıcı {userId}, {day}. günde aktifti." : $"Kullanıcı {userId}, {day}. günde aktif değildi.");
    }

    [HttpGet("active-days/{userId}")]
    public IActionResult GetActiveDays(string userId)
    {
        var key = $"{UserKey}{userId}";
        long activeDays = _redisDb.StringBitCount(key);

        return Ok($"Kullanıcı {userId}, toplam {activeDays} gün aktifti.");
    }
}
