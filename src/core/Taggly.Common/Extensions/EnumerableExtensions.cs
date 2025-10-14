namespace Taggly.Common.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Runs action for each element in sequence.
    /// </summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));
        foreach (var item in source) action(item);
    }
}
