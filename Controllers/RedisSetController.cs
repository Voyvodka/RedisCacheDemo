using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-set")]
[ApiController]
public class RedisSetController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string SetKey = "uniqueUsers";

    public RedisSetController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("add/{username}")]
    public IActionResult AddUser(string username)
    {
        _redisDb.SetAdd(SetKey, username);
        return Ok($"'{username}' set'e eklendi.");
    }

    [HttpGet("get")]
    public IActionResult GetUsers()
    {
        var users = _redisDb.SetMembers(SetKey);
        return Ok(users.ToStringArray());
    }

    [HttpGet("exists/{username}")]
    public IActionResult Exists(string username)
    {
        bool exists = _redisDb.SetContains(SetKey, username);
        return Ok(exists ? $"{username} set içinde mevcut." : $"{username} set içinde bulunamadı.");
    }

    [HttpDelete("remove/{username}")]
    public IActionResult RemoveUser(string username)
    {
        _redisDb.SetRemove(SetKey, username);
        return Ok($"'{username}' set'ten silindi.");
    }

    [HttpGet("random")]
    public IActionResult GetRandomUser()
    {
        var user = _redisDb.SetRandomMember(SetKey);
        return Ok(user.HasValue ? user.ToString() : "Set boş.");
    }
}
