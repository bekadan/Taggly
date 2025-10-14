namespace Taggly.Exceptions.Base;

public class ConflictException : TagglyException
{
    public ConflictException(string message)
        : base(message) { }
}
