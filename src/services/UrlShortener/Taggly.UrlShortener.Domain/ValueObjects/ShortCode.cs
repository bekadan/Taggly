using System.Text.RegularExpressions;
using Taggly.Common.Domain;
using Taggly.Common.Extensions;
using Taggly.Common.Types;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class ShortCode : ValueObject
{
    private static readonly Regex ValidCodeRegex = new(@"^[a-zA-Z0-9]{3,7}$", RegexOptions.Compiled);
    public string Value { get; }

    private ShortCode(string value)
    {
        Value = value;
    }

    public static Result<ShortCode> Create(string value)
        => Result.Create(value, Errors.ShortCode.ShortCodeCannotBeEmpty)
            .Ensure(s => !string.IsNullOrWhiteSpace(s), Errors.ShortCode.ShortCodeCannotBeEmpty)
            .Ensure(s=>!ValidCodeRegex.IsMatch(s), Errors.ShortCode.InvalidShortCodeFormat)
            .Map(s => new ShortCode(s));

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is ShortCode other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
