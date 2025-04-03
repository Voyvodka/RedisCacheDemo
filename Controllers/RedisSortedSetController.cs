using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisCacheDemo.Controllers;

[Route("api/redis-sortedset")]
[ApiController]
public class RedisSortedSetController : ControllerBase
{
    private readonly IDatabase _redisDb;
    private const string LeaderboardKey = "gameLeaderboard";

    public RedisSortedSetController(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    [HttpPost("add/{player}/{score}")]
    public IActionResult AddPlayer(string player, double score)
    {
        _redisDb.SortedSetAdd(LeaderboardKey, player, score);
        return Ok($"'{player}' adlı oyuncu {score} puan ile eklendi.");
    }

    [HttpGet("get")]
    public IActionResult GetLeaderboard()
    {
        var players = _redisDb.SortedSetRangeByRankWithScores(LeaderboardKey);
        return Ok(players.Select(p => new { Name = p.Element.ToString(), Score = p.Score }));
    }

    [HttpGet("get/reverse")]
    public IActionResult GetLeaderboardDescending()
    {
        var players = _redisDb.SortedSetRangeByRankWithScores(LeaderboardKey, order: Order.Descending);
        return Ok(players.Select(p => new { Name = p.Element.ToString(), Score = p.Score }));
    }

    [HttpGet("rank/{player}")]
    public IActionResult GetRank(string player)
    {
        var rank = _redisDb.SortedSetRank(LeaderboardKey, player);
        return Ok(rank.HasValue ? $"{player} sıralamada {rank.Value + 1}. sırada." : $"{player} bulunamadı.");
    }

    [HttpGet("score/{player}")]
    public IActionResult GetScore(string player)
    {
        var score = _redisDb.SortedSetScore(LeaderboardKey, player);
        return Ok(score.HasValue ? $"{player} puanı: {score.Value}" : $"{player} bulunamadı.");
    }

    [HttpPost("increase/{player}/{score}")]
    public IActionResult IncreaseScore(string player, double score)
    {
        _redisDb.SortedSetIncrement(LeaderboardKey, player, score);
        return Ok($"{player} puanı {score} artırıldı.");
    }

    [HttpDelete("remove/{player}")]
    public IActionResult RemovePlayer(string player)
    {
        _redisDb.SortedSetRemove(LeaderboardKey, player);
        return Ok($"{player} liderlik tablosundan silindi.");
    }
}
