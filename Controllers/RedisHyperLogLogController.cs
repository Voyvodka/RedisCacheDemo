using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-hyperloglog")]
[ApiController]
public class RedisHyperLogLogController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string VisitorKey = "campaign:visitors";

    public RedisHyperLogLogController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("add/{userId}")]
    public IActionResult AddVisitor(string userId)
    {
        bool isNew = _redisDb.HyperLogLogAdd(VisitorKey, userId);
        return Ok(isNew ? $"Kullanıcı {userId} eklendi." : $"Kullanıcı {userId} zaten eklenmiş.");
    }

    [HttpGet("count")]
    public IActionResult GetVisitorCount()
    {
        long count = _redisDb.HyperLogLogLength(VisitorKey);
        return Ok($"Tahmini eşsiz ziyaretçi sayısı: {count}");
    }

    [HttpPost("merge/{newKey}")]
    public IActionResult MergeCampaignData(string newKey, [FromQuery] string[] keys)
    {
        if (keys.Length < 2) return BadRequest("En az 2 anahtar belirtmelisiniz.");

        RedisKey[] redisKeys = keys.Select(k => (RedisKey)k).ToArray();
        _redisDb.HyperLogLogMerge(newKey, redisKeys);

        return Ok($"Yeni HyperLogLog '{newKey}' içinde {string.Join(", ", keys)} birleştirildi.");
    }
}
