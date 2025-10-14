namespace Taggly.SharedKernel.Extensions;

/// <summary>
/// Lightweight guard helpers for argument validation.
/// </summary>
public static class Guard
{
    public static void AgainstNull<T>(T? value, string name)
    {
        if (value is null) throw new ArgumentNullException(name);
    }

    public static void AgainstNullOrEmpty(string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException($"{name} cannot be null or empty.", name);
    }

    public static void AgainstNullOrEmpty<T>(IEnumerable<T>? value, string name)
    {
        if (value == null) throw new ArgumentNullException(name);
        using var enumerator = value.GetEnumerator();
        if (!enumerator.MoveNext()) throw new ArgumentException($"{name} cannot be empty.", name);
    }
}
