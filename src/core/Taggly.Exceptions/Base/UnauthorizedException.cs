namespace Taggly.Exceptions.Base;

public class UnauthorizedException : TagglyException
{
    public UnauthorizedException()
        : base("You are not authorized to perform this action.") { }
}
