using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Taggly.Logging.Abstractions;
using Taggly.Logging.Enums;
using Taggly.Logging.Options;
using Taggly.Logging.Providers;

namespace Taggly.Logging.Extensions;


public static class TagglyLoggerExtensions
{
    public static IServiceCollection AddTagglyLogger(this IServiceCollection services, IConfiguration configuration, string? correlationId = null)
    {
        // Bind LoggerOptions with reloadOnChange
        services.Configure<LoggerOptions>(configuration.GetSection("Logging"));

        services.AddSingleton<ITagglyLogger>(sp =>
        {
            var optionsMonitor = sp.GetRequiredService<IOptionsMonitor<LoggerOptions>>();
            var loggerOptions = optionsMonitor.CurrentValue;

            // Parse global minimum level
            Enum.TryParse(loggerOptions.MinimumLevel, true, out LogLevel globalMinLevel);

            // Providers
            var providers = new List<ILogProvider>();
            if (loggerOptions.Providers.Console.Enabled)
                providers.Add(new ConsoleLogProvider());

            if (loggerOptions.Providers.ApplicationInsights.Enabled &&
                !string.IsNullOrWhiteSpace(loggerOptions.Providers.ApplicationInsights.InstrumentationKey))
            {
                Enum.TryParse(loggerOptions.Providers.ApplicationInsights.LogLevel, true, out LogLevel aiLevel);
                providers.Add(new ApplicationInsightsProvider(
                    loggerOptions.Providers.ApplicationInsights.InstrumentationKey,
                    aiLevel));
            }

            // Enrichers
            var enrichers = new List<ILogEnricher>();
            if (loggerOptions.Enrichers.CorrelationId && !string.IsNullOrEmpty(correlationId))
                enrichers.Add(new Enrichers.CorrelationIdEnricher(correlationId));
            if (loggerOptions.Enrichers.TraceContext)
                enrichers.Add(new Enrichers.TraceContextEnricher());

            // Create logger with initial level
            var logger = new Logger(providers, enrichers, globalMinLevel);

            // Subscribe to configuration changes
            optionsMonitor.OnChange(newOptions =>
            {
                Enum.TryParse(newOptions.MinimumLevel, true, out LogLevel newLevel);
                logger.SetLogLevel(newLevel);
            });

            return logger;
        });

        return services;
    }
}

/*
{
  "Logging": {
    "MinimumLevel": "Debug",
    "Providers": {
      "Console": { "Enabled": true },
      "ApplicationInsights": {
        "Enabled": true,
        "InstrumentationKey": "YOUR-INSTRUMENTATION-KEY",
        "LogLevel": "Information"
      }
    },
    "Enrichers": {
      "CorrelationId": true,
      "TraceContext": true
    }
  }
}


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

 */
