using Taggly.Logging.Abstractions;
using Taggly.Logging.Enums;

namespace Taggly.Logging;

public class Logger : ITagglyLogger
{
    private readonly List<ILogProvider> _providers;
    private readonly List<ILogEnricher> _enrichers;
    private LogLevel _currentLevel;
    private readonly object _lock = new();

    public Logger(IEnumerable<ILogProvider> providers, IEnumerable<ILogEnricher>? enrichers, LogLevel initialLevel)
    {
        _providers = providers.ToList();
        _enrichers = enrichers?.ToList() ?? new List<ILogEnricher>();
        _currentLevel = initialLevel;
    }

    public LogLevel CurrentLogLevel
    {
        get { lock (_lock) { return _currentLevel; } }
    }

    public void SetLogLevel(LogLevel level)
    {
        lock (_lock)
        {
            _currentLevel = level;
        }
    }

    private void LogInternal(LogLevel level, string message, params object[] args)
    {
        if (level < CurrentLogLevel) return;

        var formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
        var context = new LogContext();
        foreach (var enricher in _enrichers)
            enricher.Enrich(context);

        foreach (var provider in _providers)
            provider.WriteLog(level, formattedMessage, context);
    }

    public void Log(LogLevel level, string message, params object[] args) => LogInternal(level, message, args);
    public void LogInformation(string message, params object[] args) => LogInternal(LogLevel.Information, message, args);
    public void LogWarning(string message, params object[] args) => LogInternal(LogLevel.Warning, message, args);
    public void LogError(string message, params object[] args) => LogInternal(LogLevel.Error, message, args);
    public void LogDebug(string message, params object[] args) => LogInternal(LogLevel.Debug, message, args);
    public void LogCritical(string message, params object[] args) => LogInternal(LogLevel.Critical, message, args);
}
