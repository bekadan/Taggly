namespace Taggly.Common.Extensions;

public static class EnumExtensions
{
    public static string ToStringValue<TEnum>(this TEnum value)
        where TEnum : struct, Enum
        => Enum.GetName(typeof(TEnum), value) ?? string.Empty;

    public static TEnum ParseEnum<TEnum>(this string value, bool ignoreCase = true)
        where TEnum : struct, Enum
        => Enum.TryParse<TEnum>(value, ignoreCase, out var result) ? result : default;
}
