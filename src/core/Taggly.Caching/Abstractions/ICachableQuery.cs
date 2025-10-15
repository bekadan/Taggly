namespace Taggly.Caching.Abstractions;

public interface ICachableQuery
{
    /// <summary>
    /// Unique cache key for the query.
    /// </summary>
    string CacheKey { get; }

    /// <summary>
    /// Absolute expiration in seconds.
    /// </summary>
    int? AbsoluteExpirationInSeconds { get; }
}
