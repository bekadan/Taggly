using Taggly.Common.Domain;
using Taggly.Common.Extensions;
using Taggly.Common.Types;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class OriginalUrl : ValueObject
{
    public string Value { get; }

    private OriginalUrl(string value)
    {
        Value = value;
    }

    public static Result<OriginalUrl> Create(string value)
        => Result.Create(value, Errors.ShortUrl.UrlCannotBeEmpty)
            .Ensure(f => !string.IsNullOrWhiteSpace(f), Errors.ShortUrl.UrlCannotBeEmpty)
            .Ensure(f => !Uri.IsWellFormedUriString(value, UriKind.Absolute), Errors.ShortUrl.InvalidUrlFormat)
            .Map(f => new OriginalUrl(value));

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is OriginalUrl other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
