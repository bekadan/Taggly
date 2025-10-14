namespace Taggly.Common.Extensions;

public static class DateTimeExtensions
{
    public static string ToFormattedString(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        => dateTime.ToString(format);

    public static long ToUnixTimestamp(this DateTime dateTime)
        => new DateTimeOffset(dateTime).ToUnixTimeSeconds();

    public static DateTime FromUnixTimestamp(this long timestamp)
        => DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;

    public static bool IsWithin(this DateTime date, DateTime start, DateTime end)
        => date >= start && date <= end;
}
