using Taggly.Common.Abstractions;
using Taggly.UrlShortener.Domain.Events;
using Taggly.UrlShortener.Domain.ValueObjects;

namespace Taggly.UrlShortener.Domain.Entities;

public class ShortUrl : IAggregateRoot
{
    public Guid Id { get; private set; }
    public ShortCode ShortCode { get; private set; }
    public OriginalUrl OriginalUrl { get; private set; }
    public UrlMetadata Metadata { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    // factory method
    public static ShortUrl Create(OriginalUrl originalUrl, ShortCode shortCode, UrlMetadata metadata)
    {
        var entity = new ShortUrl
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            Metadata = metadata,
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = metadata.ExpirationDate
        };

        entity.AddDomainEvent(new UrlCreatedDomainEvent(entity.Id, entity.ShortCode.Value, entity.OriginalUrl.Value));
        return entity;
    }

    // example domain behavior
    public void Expire()
    {
        ExpirationDate = DateTime.UtcNow;
    }

    // domain events collection (optional in aggregate)
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
}
