using System.Text.Json;
using Taggly.Logging.Abstractions;
using Taggly.Logging.Enums;

namespace Taggly.Logging.Providers;

public class ConsoleLogProvider : ILogProvider
{
    public void WriteLog(LogLevel level, string message, LogContext context)
    {
        var logEntry = new
        {
            Level = level.ToString(),
            Message = message,
            Timestamp = DateTime.UtcNow,
            Context = context.Properties
        };

        Console.WriteLine(JsonSerializer.Serialize(logEntry));
    }
}
