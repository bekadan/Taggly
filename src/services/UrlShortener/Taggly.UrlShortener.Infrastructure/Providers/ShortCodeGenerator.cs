using System.Security.Cryptography;
using System.Text;
using Taggly.UrlShortener.Application.Interfaces;

namespace Taggly.UrlShortener.Infrastructure.Providers;

public class ShortCodeGenerator : IShortCodeGenerator
{
    private const string AllowedChars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    public string Generate(int length)
    {
        // Calculate how many bytes we need for the requested length
        // We request more bytes than needed to ensure good randomness
        var maxNumber = AllowedChars.Length;
        var bytes = RandomNumberGenerator.GetBytes(length * 8);
        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            // Use 4 bytes to generate a random number
            var value = BitConverter.ToUInt32(bytes, i * 4);
            // Map the number to our allowed character range
            result.Append(AllowedChars[(int)(value % maxNumber)]);
        }

        return result.ToString();
    }
}
