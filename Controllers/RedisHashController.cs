using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-hash")]
[ApiController]
public class RedisHashController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string UserKey = "user:";

    public RedisHashController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("add/{userId}")]
    public IActionResult AddUser(string userId, [FromBody] UserDto user)
    {
        var key = $"{UserKey}{userId}";
        _redisDb.HashSet(key, new HashEntry[]
        {
            new("name", user.Name),
            new("age", user.Age.ToString()),
            new("city", user.City)
        });

        return Ok($"Kullanıcı {userId} başarıyla eklendi.");
    }

    [HttpGet("get/{userId}")]
    public IActionResult GetUser(string userId)
    {
        var key = $"{UserKey}{userId}";
        var user = _redisDb.HashGetAll(key);

        if (user.Length == 0) return NotFound("Kullanıcı bulunamadı.");

        return Ok(user.ToDictionary(
            x => x.Name.ToString(),
            x => x.Value.ToString()
        ));
    }

    // Kullanıcıdan Belirli Alanı Getirme
    [HttpGet("get/{userId}/{field}")]
    public IActionResult GetUserField(string userId, string field)
    {
        var key = $"{UserKey}{userId}";
        var value = _redisDb.HashGet(key, field);

        if (value.IsNullOrEmpty) return NotFound($"{field} bulunamadı.");

        return Ok($"{field}: {value}");
    }

    // Kullanıcıdan Belirli Alanı Silme
    [HttpDelete("delete/{userId}/{field}")]
    public IActionResult DeleteUserField(string userId, string field)
    {
        var key = $"{UserKey}{userId}";
        bool removed = _redisDb.HashDelete(key, field);

        return removed ? Ok($"{field} silindi.") : NotFound($"{field} bulunamadı.");
    }

    // Kullanıcı Yaşını Artırma
    [HttpPost("increment-age/{userId}/{amount}")]
    public IActionResult IncrementAge(string userId, int amount)
    {
        var key = $"{UserKey}{userId}";
        var newAge = _redisDb.HashIncrement(key, "age", amount);
        return Ok($"Yeni yaş: {newAge}");
    }

    // Kullanıcıyı Tamamen Silme
    [HttpDelete("delete/{userId}")]
    public IActionResult DeleteUser(string userId)
    {
        var key = $"{UserKey}{userId}";
        return _redisDb.KeyDelete(key) ? Ok("Kullanıcı silindi.") : NotFound("Kullanıcı bulunamadı.");
    }
}

public class UserDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string City { get; set; }
}
