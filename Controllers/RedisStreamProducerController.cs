using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-streams")]
[ApiController]
public class RedisStreamProducerController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string StreamKey = "mystream";

    public RedisStreamProducerController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("add-event")]
    public IActionResult AddEvent(string eventType, string userId)
    {
        var messageId = _redisDb.StreamAdd(StreamKey, new NameValueEntry[]
        {
            new("eventType", eventType),
            new("userId", userId)
        });

        return Ok($"Event eklendi: {messageId}");
    }
}