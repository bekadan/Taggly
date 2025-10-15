using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taggly.Caching.Abstractions;
using Taggly.Caching.Behaviours;
using Taggly.Caching.Configuration;
using Taggly.Caching.Handlers;
using Taggly.Caching.Management;
using Taggly.Caching.Services;

namespace Taggly.Caching.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTagglyCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheOptions = new CacheOptions();
        configuration.GetSection("Cache").Bind(cacheOptions);

        services.AddSingleton(cacheOptions);
        services.AddSingleton<ICacheService, RedisCacheService>();
        services.AddSingleton<ICacheManager, RedisCacheManager>();

        // Add caching behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));

        // Register the invalidation handler for all ICacheInvalidationEvent types
        services.AddTransient(typeof(INotificationHandler<>), typeof(CacheInvalidationHandler<>));

        return services;
    }
}

/*
 {
  "Cache": {
    "Provider": "Redis",
    "RedisConnectionString": "localhost:6379",
    "DefaultExpirationSeconds": 600
  }
}
 */