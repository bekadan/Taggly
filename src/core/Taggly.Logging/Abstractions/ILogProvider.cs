using Taggly.Logging.Enums;

namespace Taggly.Logging.Abstractions;

public interface ILogProvider
{
    void WriteLog(LogLevel level, string message, LogContext context);
}
