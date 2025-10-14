using Taggly.Common.Abstractions;

namespace Taggly.UrlShortener.Domain.Events;

public sealed class UrlCreatedDomainEvent : IDomainEvent
{
    public Guid ShortUrlId { get; }
    public string ShortCode { get; }
    public string OriginalUrl { get; }
    public DateTime OccurredOn { get; }

    public UrlCreatedDomainEvent(Guid shortUrlId, string shortCode, string originalUrl)
    {
        ShortUrlId = shortUrlId;
        ShortCode = shortCode;
        OriginalUrl = originalUrl;
    }
}
