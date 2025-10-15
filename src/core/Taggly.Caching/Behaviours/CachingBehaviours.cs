using MediatR;
using Taggly.Caching.Abstractions;
using Taggly.Caching.Configuration;

namespace Taggly.Caching.Behaviours;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheService _cache;
    private readonly CacheOptions _options;

    public CachingBehavior(ICacheService cache, CacheOptions options)
    {
        _cache = cache;
        _options = options;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICachableQuery cachableQuery)
            return await next();

        var cacheKey = cachableQuery.CacheKey;
        var cached = await _cache.GetAsync<TResponse>(cacheKey);

        if (cached is not null)
            return cached;

        var response = await next();

        var expiration = cachableQuery.AbsoluteExpirationInSeconds.HasValue
            ? TimeSpan.FromSeconds(cachableQuery.AbsoluteExpirationInSeconds.Value)
            : TimeSpan.FromSeconds(_options.DefaultExpirationSeconds);

        await _cache.SetAsync(cacheKey, response, expiration);

        return response;
    }
}
