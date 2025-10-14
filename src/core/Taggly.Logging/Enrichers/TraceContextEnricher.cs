using System.Diagnostics;
using Taggly.Logging.Abstractions;

namespace Taggly.Logging.Enrichers;

public class TraceContextEnricher : ILogEnricher
{
    public void Enrich(LogContext context)
    {
        var activity = Activity.Current;
        if (activity != null)
        {
            context.Properties["TraceId"] = activity.TraceId.ToString();
            context.Properties["SpanId"] = activity.SpanId.ToString();
        }
    }
}
