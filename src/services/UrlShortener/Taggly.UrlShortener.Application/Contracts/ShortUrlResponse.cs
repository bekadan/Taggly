using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Contracts;

public sealed record ShortUrlResponse
{

    public Guid Id { get; set; }
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public string? Description { get; set; }

    public ShortUrlResponse(ShortUrl ShortUrl)
    {
        Id = ShortUrl.Id;
        ShortCode = ShortUrl.ShortCode.Value;
        OriginalUrl = ShortUrl.OriginalUrl.Value;
        CreatedBy = ShortUrl.Metadata.CreatedBy;
        ExpirationDate = ShortUrl.Metadata.ExpirationDate;
        Description = ShortUrl.Metadata.Description;
    }
}
