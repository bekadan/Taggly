namespace Taggly.Caching.Configuration;

public class CacheOptions
{
    public string Provider { get; set; } = "Redis";
    public string RedisConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationSeconds { get; set; } = 600;
}
