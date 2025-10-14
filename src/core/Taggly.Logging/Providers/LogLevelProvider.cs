using Taggly.Logging.Abstractions;
using Taggly.Logging.Enums;

namespace Taggly.Logging.Providers;

public class LogLevelProvider : ILogLevelProvider
{
    private LogLevel _currentLevel;
    private readonly object _lock = new();

    public LogLevelProvider(LogLevel initialLevel)
    {
        _currentLevel = initialLevel;
    }

    public LogLevel CurrentLevel
    {
        get
        {
            lock (_lock) { return _currentLevel; }
        }
    }

    public void SetLevel(LogLevel level)
    {
        lock (_lock)
        {
            _currentLevel = level;
        }
    }
}
