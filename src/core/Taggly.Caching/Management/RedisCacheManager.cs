using StackExchange.Redis;
using System.Text.Json;
using Taggly.Caching.Configuration;

namespace Taggly.Caching.Management;

public class RedisCacheManager : ICacheManager, IDisposable
{
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly JsonSerializerOptions _serializerOptions;

    public RedisCacheManager(CacheOptions options)
    {
        _redis = ConnectionMultiplexer.Connect(options.RedisConnectionString);
        _database = _redis.GetDatabase();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    public async Task<IEnumerable<string>> GetKeysAsync(string pattern = "*", int limit = 1000)
    {
        var endpoints = _redis.GetEndPoints();
        var allKeys = new List<string>();

        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            var keys = server.Keys(pattern: pattern)
                .Take(limit)
                .Select(k => (string?)k)
                .Where(k => k != null)
                .Select(k => k!); // Ensure non-null
            allKeys.AddRange(keys);
        }

        return await Task.FromResult(allKeys);
    }

    public async Task<long> GetKeyCountAsync(string pattern = "*")
    {
        var endpoints = _redis.GetEndPoints();
        long count = 0;

        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            count += server.Keys(pattern: pattern).LongCount();
        }

        return await Task.FromResult(count);
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!, _serializerOptions);
    }

    public async Task<bool> RemoveKeyAsync(string key)
    {
        return await _database.KeyDeleteAsync(key);
    }

    public async Task<long> RemoveByPatternAsync(string pattern)
    {
        long removed = 0;
        var endpoints = _redis.GetEndPoints();

        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            var keys = server.Keys(pattern: pattern).ToArray();
            if (keys.Length > 0)
                removed += await _database.KeyDeleteAsync(keys);
        }

        return removed;
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    public void Dispose()
    {
        _redis.Dispose();
    }
}
