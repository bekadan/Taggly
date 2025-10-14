using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Taggly.Logging.Abstractions;
using Taggly.Logging.Enums;

namespace Taggly.Logging.Providers;

public class ApplicationInsightsProvider : ILogProvider
{
    private readonly TelemetryClient _telemetryClient;
    private readonly LogLevel _minLevel;

    public ApplicationInsightsProvider(string connectionString, LogLevel minLevel = LogLevel.Information)
    {
        var config = TelemetryConfiguration.CreateDefault();
        config.ConnectionString = connectionString;
        _telemetryClient = new TelemetryClient(config);
        _minLevel = minLevel;
    }

    public void WriteLog(LogLevel level, string message, LogContext context)
    {
        if (level < _minLevel) return;

        var properties = context.Properties.ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty);

        switch (level)
        {
            case LogLevel.Critical:
            case LogLevel.Error:
                _telemetryClient.TrackException(new Exception(message), properties);
                break;
            case LogLevel.Warning:
                _telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning, properties);
                break;
            case LogLevel.Information:
                _telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information, properties);
                break;
            case LogLevel.Debug:
            case LogLevel.Trace:
                _telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose, properties);
                break;
        }

        _telemetryClient.Flush();
    }
}
