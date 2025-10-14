namespace Taggly.Exceptions.Base;

public abstract class TagglyException : Exception
{
    protected TagglyException() { }
    protected TagglyException(string message) : base(message) { }
    protected TagglyException(string message, Exception innerException)
        : base(message, innerException) { }
}
