using Microsoft.AspNetCore.Mvc;
using RedisCacheDemo.Services;

namespace RedisCacheDemo.Area.Demo1.Controllers;
[Route("api/user")]
[ApiController]
public class UserProfileController : ControllerBase
{
    private readonly RedisCacheService _redisCacheService;
    private const string ProfileKeyPrefix = "user_profile:";
    private const string ActivityKeyPrefix = "user_activity:";
    private const string PreferencesKeyPrefix = "user_preferences:";
    private const string ReportKeyPrefix = "user_report:";

    public UserProfileController(RedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    [HttpPost("set-profile/{userId}")]
    public async Task<IActionResult> SetProfile(string userId, [FromBody] UserProfile profile)
    {
        var key = ProfileKeyPrefix + userId;
        await _redisCacheService.SetObjectAsync(key, profile);
        return Ok(new { Message = "Profile cached." });
    }

    [HttpGet("get-profile/{userId}")]
    public async Task<IActionResult> GetProfile(string userId)
    {
        var key = ProfileKeyPrefix + userId;
        var profile = await _redisCacheService.GetObjectAsync<UserProfile>(key);
        if (profile == null)
            return NotFound();

        return Ok(profile);
    }

    [HttpPost("set-activity/{userId}")]
    public async Task<IActionResult> SetActivity(string userId, [FromBody] UserActivity activity)
    {
        var key = ActivityKeyPrefix + userId;
        await _redisCacheService.SetObjectAsync(key, activity);
        return Ok(new { Message = "Activity cached." });
    }

    [HttpGet("get-activity/{userId}")]
    public async Task<IActionResult> GetActivity(string userId)
    {
        var key = ActivityKeyPrefix + userId;
        var activity = await _redisCacheService.GetObjectAsync<UserActivity>(key);
        if (activity == null)
            return NotFound();

        return Ok(activity);
    }

    [HttpPost("set-preferences/{userId}")]
    public async Task<IActionResult> SetPreferences(string userId, [FromBody] UserPreferences preferences)
    {
        var key = PreferencesKeyPrefix + userId;
        await _redisCacheService.SetObjectAsync(key, preferences);
        return Ok(new { Message = "Preferences cached." });
    }

    [HttpGet("get-preferences/{userId}")]
    public async Task<IActionResult> GetPreferences(string userId)
    {
        var key = PreferencesKeyPrefix + userId;
        var preferences = await _redisCacheService.GetObjectAsync<UserPreferences>(key);
        if (preferences == null)
            return NotFound();

        return Ok(preferences);
    }

    [HttpPost("set-report/{userId}")]
    public async Task<IActionResult> SetReport(string userId, [FromBody] byte[] reportPdf)
    {
        var key = ReportKeyPrefix + userId;
        await _redisCacheService.SetCacheAsync(key, Convert.ToBase64String(reportPdf));
        return Ok(new { Message = "Report cached." });
    }

    [HttpGet("get-report/{userId}")]
    public async Task<IActionResult> GetReport(string userId)
    {
        var key = ReportKeyPrefix + userId;
        var reportBase64 = await _redisCacheService.GetCacheAsync(key);
        if (reportBase64 == null)
            return NotFound();

        var reportPdf = Convert.FromBase64String(reportBase64);
        return File(reportPdf, "application/pdf", "user_report.pdf");
    }
}

public class UserProfile
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}

public class UserActivity
{
    public DateTime LastLogin { get; set; }
    public string LastAction { get; set; } = string.Empty;
}

public class UserPreferences
{
    public bool IsEmailSubscribed { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
}