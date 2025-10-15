using Taggly.Common.Domain;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class UrlMetadata : ValueObject
{
    public Guid? CreatedBy { get; }
    public DateTime? ExpirationDate { get; }
    public string? Description { get; }

    private UrlMetadata(Guid? createdBy, DateTime? expirationDate = null, string? description = null)
    {
        if (expirationDate.HasValue && expirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.", nameof(expirationDate));

        Description = description;
        CreatedBy = createdBy;
        ExpirationDate = expirationDate;
    }

    public static UrlMetadata Create(Guid? createdBy = null, DateTime? expirationDate = null, string? description = null)
        => new(createdBy, expirationDate, description);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CreatedBy ?? Guid.Empty;
        yield return ExpirationDate ?? DateTime.MinValue;
        yield return Description ?? string.Empty;
    }
}
