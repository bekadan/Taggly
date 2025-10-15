using System.Security.Cryptography;
using System.Text;
using Taggly.UrlShortener.Application.Interfaces;

namespace Taggly.UrlShortener.Infrastructure.Providers;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";

    public string Generate(int length)
    {
        if (length < 3 || length > 7)
            throw new ArgumentOutOfRangeException(nameof(length), "Short code length must be between 4 and 32 characters.");

        var bytes = RandomNumberGenerator.GetBytes(length);
        var result = new StringBuilder(length);

        foreach (var b in bytes)
            result.Append(AllowedChars[b % AllowedChars.Length]);

        return result.ToString();
    }
}
