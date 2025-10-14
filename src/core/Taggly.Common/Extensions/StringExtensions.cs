using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Taggly.Common.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? value)
        => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(this string? value)
        => string.IsNullOrWhiteSpace(value);

    public static string ToSlug(this string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;
        var normalized = input.ToLowerInvariant().Trim();
        normalized = Regex.Replace(normalized, @"[^a-z0-9\s-]", "");
        normalized = Regex.Replace(normalized, @"\s+", "-");
        return normalized;
    }

    public static string Base64Encode(this string plainText)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

    public static string Base64Decode(this string base64Encoded)
        => Encoding.UTF8.GetString(Convert.FromBase64String(base64Encoded));

    public static string Capitalize(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value);
    }
}
