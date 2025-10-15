using Taggly.Common.Domain;

namespace Taggly.UrlShortener.Domain;

public static class Errors
{
    public static Error ShortUrlNotFound
        => new Error("ShortUrlNotFound", "Short Url Not Found!");

    public static Error ShortUrlExpired
        => new Error("ShortUrlExpired", "Short Url is Expired!");
}
