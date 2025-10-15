using Taggly.Common.Domain;
using Taggly.UrlShortener.Domain.Events;
using Taggly.UrlShortener.Domain.ValueObjects;

namespace Taggly.UrlShortener.Domain.Entities;

public class ShortUrl : AggregateRoot
{
    public ShortCode ShortCode { get; private set; }
    public OriginalUrl OriginalUrl { get; private set; }
    public UrlMetadata Metadata { get; private set; }

    // EF Core requires a parameterless constructor
    private ShortUrl() { }

    // factory method
    private ShortUrl(OriginalUrl originalUrl, ShortCode shortCode, UrlMetadata metadata)
    {
        Id = Guid.NewGuid();
        OriginalUrl = originalUrl ?? throw new ArgumentNullException(nameof(originalUrl));
        ShortCode = shortCode ?? throw new ArgumentNullException(nameof(shortCode));
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));

        AddDomainEvent(new UrlCreatedDomainEvent(Id, ShortCode.Value, OriginalUrl.Value));
    }

    public static ShortUrl Create(OriginalUrl originalUrl, ShortCode shortCode, UrlMetadata metadata)
            => new ShortUrl(originalUrl, shortCode, metadata);
}
