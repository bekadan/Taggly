using Taggly.Common.Abstractions;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class OriginalUrl : IValueObject
{
    public string Value { get; }

    private OriginalUrl(string value)
    {
        Value = value;
    }

    public static OriginalUrl Create(string value)
    {
        if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
            throw new ArgumentException("Invalid URL format.", nameof(value));

        return new OriginalUrl(value);
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is OriginalUrl other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
