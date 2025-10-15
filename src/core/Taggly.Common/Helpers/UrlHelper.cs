using System.Text.RegularExpressions;

namespace Taggly.Common.Helpers;

public static class UrlHelper
{
    public static bool IsValidUrl(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
            return false;

        if (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            return false;

        // Simple regex to check domain: must contain a dot and valid TLD
        string hostPattern = @"^([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$";
        return Regex.IsMatch(uriResult.Host, hostPattern);
    }

    public static bool IsUrlReachableSync(string url)
    {
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5); // short timeout for responsiveness

            var response = client.GetAsync(url).GetAwaiter().GetResult();
            return response.IsSuccessStatusCode; // true if 200–299
        }
        catch
        {
            return false; // network errors or invalid URL
        }
    }

    public static string Combine(string baseUrl, string path)
    {
        if (string.IsNullOrWhiteSpace(baseUrl)) return path;
        if (string.IsNullOrWhiteSpace(path)) return baseUrl;

        baseUrl = baseUrl.TrimEnd('/');
        path = path.TrimStart('/');
        return $"{baseUrl}/{path}";
    }
}
