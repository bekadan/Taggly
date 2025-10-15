using MediatR;

namespace Taggly.Caching.Abstractions;

public interface ICacheInvalidationEvent : INotification
{
    /// <summary>
    /// Keys or patterns of cache entries to remove.
    /// </summary>
    IReadOnlyCollection<string> KeysOrPatterns { get; }
    CacheInvalidationType InvalidationType { get; }
}
