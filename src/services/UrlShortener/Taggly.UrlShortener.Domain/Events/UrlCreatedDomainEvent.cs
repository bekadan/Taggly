using Taggly.Common.Abstractions;

namespace Taggly.UrlShortener.Domain.Events;

public sealed class UrlCreatedDomainEvent : IDomainEvent
{
    public Guid Id { get; }
    public string ShortCode { get; }
    public string OriginalUrl { get; }
    public DateTime OccurredOn { get; }

    public UrlCreatedDomainEvent(Guid id, string shortCode, string originalUrl)
    {
        Id = id;
        ShortCode = shortCode ?? throw new ArgumentNullException(nameof(shortCode)); ;
        OriginalUrl = originalUrl ?? throw new ArgumentNullException(nameof(originalUrl));
        OccurredOn = DateTime.UtcNow;
    }
}
