namespace Taggly.Exceptions.Base;

public class TagglyProblemDetails
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public string Instance { get; set; }
    public IDictionary<string, object> Errors { get; set; } = new Dictionary<string, object>();
}
