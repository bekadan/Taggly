using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Taggly.UrlShortener.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR((c) =>
        {
            c.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            c.LicenseKey = configuration.GetSection("MediatR:LicenseKey").Value;
        });

        return services;
    }
}
