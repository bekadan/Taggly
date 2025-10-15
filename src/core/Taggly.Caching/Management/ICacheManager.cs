namespace Taggly.Caching.Management;

public interface ICacheManager
{
    Task<IEnumerable<string>> GetKeysAsync(string pattern = "*", int limit = 1000);
    Task<long> GetKeyCountAsync(string pattern = "*");
    Task<T?> GetValueAsync<T>(string key);
    Task<bool> RemoveKeyAsync(string key);
    Task<long> RemoveByPatternAsync(string pattern);
    Task<bool> ExistsAsync(string key);
}
