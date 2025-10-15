using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taggly.Common.Abstractions.Repositories;

using Taggly.UrlShortener.Domain.Entities;
using Taggly.UrlShortener.Infrastructure.Persistence;
using Taggly.UrlShortener.Infrastructure.Repositories;

namespace Taggly.UrlShortener.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UrlShortenerDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("UrlShortenerDbConnectionString"))
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped(typeof(IAsyncRepository<ShortUrl>), typeof(AsyncRepositoryBase<ShortUrl, UrlShortenerDbContext>));
        return services;
    }
}
