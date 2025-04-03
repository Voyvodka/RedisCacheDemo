using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis")]
[ApiController]
public class RedisController : ControllerBase
{
    private readonly IDatabase _redisDb;

    public RedisController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpGet("set/{key}/{value}")]
    public IActionResult SetString(string key, string value)
    {
        _redisDb.StringSet(key, value);
        return Ok($"Key '{key}' set with value '{value}'");
    }

    [HttpGet("get/{key}")]
    public IActionResult GetString(string key)
    {
        var value = _redisDb.StringGet(key);
        if (value.IsNullOrEmpty)
            return NotFound($"Key '{key}' not found");

        return Ok(value.ToString());
    }
}
