namespace Taggly.Logging.Abstractions;

public interface ILogEnricher
{
    void Enrich(LogContext context);
}

public class LogContext
{
    public Dictionary<string, object> Properties { get; } = new();
}
