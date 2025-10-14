using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taggly.Common.Enums;

namespace Taggly.Common.Config;

/// <summary>
/// Common service registration helpers for DI.
/// </summary>
public static class CommonServiceCollectionExtensions
{
    public static IServiceCollection AddTagglyCommon(this IServiceCollection services, IConfiguration configuration)
    {
        // Here we could register global utilities, options, and environment info.
        var environment = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";
        services.AddSingleton(new { Environment = environment });
        return services;
    }

    public static EnvironmentType GetEnvironment(this IConfiguration configuration)
    {
        var env = configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development";
        return env switch
        {
            "Production" => EnvironmentType.Production,
            "Staging" => EnvironmentType.Staging,
            _ => EnvironmentType.Development
        };
    }
}
