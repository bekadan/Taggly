using Taggly.Logging.Enums;

namespace Taggly.Logging.Abstractions;

public interface ITagglyLogger
{
    void Log(LogLevel level, string message, params object[] args);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message, params object[] args);
    void LogError(string message, params object[] args);
    void LogDebug(string message, params object[] args);
    void LogCritical(string message, params object[] args);

    // New runtime log level methods
    LogLevel CurrentLogLevel { get; }
    void SetLogLevel(LogLevel level);
}
