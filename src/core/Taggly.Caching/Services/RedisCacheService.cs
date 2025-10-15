using StackExchange.Redis;
using System.Text.Json;
using Taggly.Caching.Abstractions;
using Taggly.Caching.Configuration;

namespace Taggly.Caching.Services;

public class RedisCacheService : ICacheService, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _serializerOptions;

    public RedisCacheService(CacheOptions options)
    {
        _redis = ConnectionMultiplexer.Connect(options.RedisConnectionString);
        _database = _redis.GetDatabase();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!, _serializerOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var serialized = JsonSerializer.Serialize(value, _serializerOptions);
        await _database.StringSetAsync(key, serialized, absoluteExpiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        var endpoints = _redis.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            var keys = server.Keys(pattern: pattern);
            var keyBatch = keys as RedisKey[] ?? keys.ToArray();
            if (keyBatch.Length > 0)
                await _database.KeyDeleteAsync(keyBatch);
        }
    }

    // Pub/Sub Support
    public void SubscribeToInvalidationChannel(string channel = "cache-invalidation")
    {
        var subscriber = _redis.GetSubscriber();
        subscriber.Subscribe(RedisChannel.Pattern(channel), async (_, message) =>
        {
            await RemoveByPatternAsync(message!);
        });
    }

    public async Task PublishInvalidationAsync(string pattern, string channel = "cache-invalidation")
    {
        var subscriber = _redis.GetSubscriber();
        await subscriber.PublishAsync(RedisChannel.Pattern(channel), pattern);
    }

    public void Dispose()
    {
        _redis.Dispose();
    }
}
