using Taggly.Common.Domain;

namespace Taggly.UrlShortener.Domain;

public static class Errors
{
    public static class ShortUrl
    {
        public static Error ShortUrlNotFound
        => new Error("ShortUrlNotFound", "Short Url Not Found!");

        public static Error ShortUrlExpired
            => new Error("ShortUrlExpired", "Short Url is Expired!");

        public static Error UrlCannotBeEmpty
            => new Error("UrlCannotBeEmpty", "URL cannot be empty.");

        public static Error InvalidUrlFormat
            => new Error("InvalidUrlFormat", "Invalid URL format.");

        public static Error ShortUrlCouldntCreated
            => new Error("ShortUrlCouldntCreated", "Short URL couldn't be created after multiple attempts.");
    }    

    public static class ShortCode
    {
        public static Error ShortCodeNotFound
            => new Error("ShortCodeNotFound", "Short Code Not Found!");

        public static Error ShortCodeCannotBeEmpty
            => new Error("ShortCodeCannotBeEmpty", "Short code cannot be empty.");

        public static Error InvalidShortCodeFormat
            => new Error("InvalidShortCodeFormat", "Short code must be alphanumeric and between 3 to 7 characters.");
    }
}
