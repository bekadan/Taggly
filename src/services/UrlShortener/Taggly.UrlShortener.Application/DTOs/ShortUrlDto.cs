namespace Taggly.UrlShortener.Application.DTOs;

public sealed class ShortUrlDto
{
    public string ShortCode { get; init; } = default!;
    public string OriginalUrl { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime? ExpirationDate { get; init; }
}
