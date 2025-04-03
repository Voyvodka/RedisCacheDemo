using StackExchange.Redis;

namespace RedisCacheDemo.Workers;

public class RedisStreamWorker : BackgroundService
{
    private readonly IDatabase _redisDb;
    private const string StreamKey = "mystream";
    private const string GroupName = "mygroup";
    private const string ConsumerName = "consumer-1";
    private const int MaxRetryCount = 3;
    private const int RetryDelayMilliseconds = 2000;

    public RedisStreamWorker(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureConsumerGroupAsync();

        while (!stoppingToken.IsCancellationRequested)
        {
            var messages = _redisDb.StreamReadGroup(StreamKey, GroupName, ConsumerName, ">", count: 1);

            if (messages.Length > 0)
            {
                foreach (var message in messages)
                {
                    int retryCount = 0;
                    bool success = false;

                    while (retryCount < MaxRetryCount && !success)
                    {
                        try
                        {
                            Console.WriteLine($"Yeni mesaj alındı: {message.Id}");
                            foreach (var entry in message.Values)
                            {
                                Console.WriteLine($"{entry.Name}: {entry.Value}");
                            }

                            _redisDb.StreamAcknowledge(StreamKey, GroupName, message.Id);
                            success = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Hata oluştu: {ex.Message}");

                            retryCount++;
                            if (retryCount < MaxRetryCount)
                            {
                                Console.WriteLine($"Yeniden denemek için bekleniyor... {RetryDelayMilliseconds}ms");
                                await Task.Delay(RetryDelayMilliseconds, stoppingToken);
                            }
                            else
                            {
                                Console.WriteLine($"Maksimum deneme sayısına ulaşıldı. Dead Letter Queue'ya gönderilecek.");
                                await SendToDeadLetterQueue(message);
                            }
                        }
                    }
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task EnsureConsumerGroupAsync()
    {
        var streamExists = await _redisDb.KeyExistsAsync(StreamKey);
        if (!streamExists)
        {
            await _redisDb.StreamAddAsync(StreamKey, new NameValueEntry[] { new NameValueEntry("init", "start") });
            Console.WriteLine("Stream 'mystream' oluşturuldu.");
        }

        var groupExists = await _redisDb.StreamGroupInfoAsync(StreamKey);
        if (groupExists.All(group => group.Name != GroupName))
        {
            try
            {
                await _redisDb.StreamCreateConsumerGroupAsync(StreamKey, GroupName, "$");
                Console.WriteLine($"Consumer group '{GroupName}' oluşturuldu.");
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"Grup oluşturulamadı: {ex.Message}");
            }
        }
    }

    private async Task SendToDeadLetterQueue(StreamEntry message)
    {
        var dlqKey = "mystream-dlq";
        await _redisDb.StreamAddAsync(dlqKey, message.Values.Select(v => new NameValueEntry(v.Name, v.Value)).ToArray());
        Console.WriteLine($"Mesaj Dead Letter Queue'ya gönderildi: {message.Id}");
    }
}
