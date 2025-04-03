using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-streams")]
[ApiController]
public class RedisStreamConsumerController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string StreamKey = "mystream";

    public RedisStreamConsumerController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpGet("read-events")]
    public IActionResult ReadEvents()
    {
        var entries = _redisDb.StreamRead(StreamKey, "0-0");
        var events = entries.Select(e => new
        {
            Id = e.Id,
            Values = e.Values.ToDictionary(v => v.Name, v => v.Value.ToString())
        });

        return Ok(events);
    }
}
