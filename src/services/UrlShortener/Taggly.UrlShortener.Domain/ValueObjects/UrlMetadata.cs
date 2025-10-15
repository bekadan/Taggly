using Taggly.Common.Domain;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class UrlMetadata : ValueObject
{
    public string? CreatedBy { get; }
    public DateTime? ExpirationDate { get; }
    public string? Description { get; }

    private UrlMetadata(string? createdBy, DateTime? expirationDate = null, string? description = null)
    {
        if (expirationDate.HasValue && expirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.", nameof(expirationDate));

        Description = description;
        CreatedBy = createdBy;
        ExpirationDate = expirationDate;
    }

    public static UrlMetadata Create(string? createdBy = null, DateTime? expirationDate = null)
        => new(createdBy, expirationDate);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CreatedBy ?? string.Empty;
        yield return ExpirationDate ?? DateTime.MinValue;
    }
}
