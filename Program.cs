using RedisCacheDemo.Workers;
using RedisCacheDemo.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var redis = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "DemoApp_";
});


builder.Services.AddSingleton<IConnectionMultiplexer>(redis);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();

builder.Services.AddHostedService<RedisStreamWorker>();
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();