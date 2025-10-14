using Taggly.Common.Abstractions;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class ShortCode : IValueObject
{
    public string Value { get; }

    private ShortCode(string value)
    {
        Value = value;
    }

    public static ShortCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Short code cannot be empty", nameof(value));

        if (value.Length < 5 || value.Length > 15)
            throw new ArgumentException("Short code length must be between 5 and 15 characters.", nameof(value));

        return new ShortCode(value);
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ShortCode other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
