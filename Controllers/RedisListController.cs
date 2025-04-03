using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-list")]
[ApiController]
public class RedisListController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string ListKey = "taskList";

    public RedisListController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("lpush/{task}")]
    public IActionResult LPush(string task)
    {
        _redisDb.ListLeftPush(ListKey, task);
        return Ok($"Task '{task}' başa eklendi");
    }

    [HttpPost("rpush/{task}")]
    public IActionResult RPush(string task)
    {
        _redisDb.ListRightPush(ListKey, task);
        return Ok($"Task '{task}' sona eklendi");
    }

    [HttpGet("get")]
    public IActionResult GetList()
    {
        var list = _redisDb.ListRange(ListKey);
        return Ok(list.ToStringArray());
    }

    [HttpGet("lpop")]
    public IActionResult LPop()
    {
        var task = _redisDb.ListLeftPop(ListKey);
        return task.IsNullOrEmpty ? NotFound("Liste boş") : Ok($"Çekilen görev: {task}");
    }

    [HttpGet("rpop")]
    public IActionResult RPop()
    {
        var task = _redisDb.ListRightPop(ListKey);
        return task.IsNullOrEmpty ? NotFound("Liste boş") : Ok($"Çekilen görev: {task}");
    }
}
