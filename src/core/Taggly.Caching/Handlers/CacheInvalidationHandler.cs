using MediatR;
using Taggly.Caching.Abstractions;
using Taggly.Caching.Services;

namespace Taggly.Caching.Handlers;

// Ensure TEvent implements INotification as required by MediatR
public class CacheInvalidationHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : ICacheInvalidationEvent
{
    private readonly ICacheService _cache;

    public CacheInvalidationHandler(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        switch (notification.InvalidationType)
        {
            case CacheInvalidationType.Exact:
                foreach (var key in notification.KeysOrPatterns)
                {
                    await _cache.RemoveAsync(key);
                    if (_cache is RedisCacheService redis)
                        await redis.PublishInvalidationAsync(key);
                }
                break;

            case CacheInvalidationType.Pattern:
                foreach (var pattern in notification.KeysOrPatterns)
                {
                    if (_cache is RedisCacheService redis)
                    {
                        await redis.RemoveByPatternAsync(pattern);
                        await redis.PublishInvalidationAsync(pattern);
                    }
                }
                break;
        }
    }
}
