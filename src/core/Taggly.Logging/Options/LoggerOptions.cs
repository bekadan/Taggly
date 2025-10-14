namespace Taggly.Logging.Options;

public class LoggerOptions
{
    public string MinimumLevel { get; set; } = "Information";

    public ProviderOptions Providers { get; set; } = new();
    public EnricherOptions Enrichers { get; set; } = new();
}

public class ProviderOptions
{
    public ConsoleOptions Console { get; set; } = new();
    public ApplicationInsightsOptions ApplicationInsights { get; set; } = new();
}

public class ConsoleOptions
{
    public bool Enabled { get; set; } = true;
}

public class ApplicationInsightsOptions
{
    public bool Enabled { get; set; } = false;
    public string InstrumentationKey { get; set; } = string.Empty;
    public string LogLevel { get; set; } = "Information";
}

public class EnricherOptions
{
    public bool CorrelationId { get; set; } = true;
    public bool TraceContext { get; set; } = true;
}
