namespace Taggly.Exceptions.Base;

public class InfrastructureException : TagglyException
{
    public InfrastructureException(string message, Exception innerException = null)
        : base(message, innerException) { }
}