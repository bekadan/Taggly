namespace Taggly.Exceptions.Base;

public class NotFoundException : TagglyException
{
    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" ({key}) was not found.") { }
}
