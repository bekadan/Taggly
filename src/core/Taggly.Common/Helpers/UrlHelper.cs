namespace Taggly.Common.Helpers;

public static class UrlHelper
{
    public static bool IsValidUrl(string url)
        => Uri.TryCreate(url, UriKind.Absolute, out var result)
           && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);

    public static string Combine(string baseUrl, string path)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) return path;
        if (string.IsNullOrWhiteSpace(path)) return baseUrl;

        baseUrl = baseUrl.TrimEnd('/');
        path = path.TrimStart('/');
        return $"{baseUrl}/{path}";
    }
}
