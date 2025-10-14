using Taggly.Logging.Abstractions;

namespace Taggly.Logging.Enrichers;

public class CorrelationIdEnricher : ILogEnricher
{
    private readonly string _correlationId;

    public CorrelationIdEnricher(string correlationId)
    {
        _correlationId = correlationId;
    }

    public void Enrich(LogContext context)
    {
        context.Properties["CorrelationId"] = _correlationId;
    }
}
