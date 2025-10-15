using Taggly.Common.Domain;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class UrlMetadata : ValueObject
{
    public string? CreatedBy { get; }
    public DateTime? ExpirationDate { get; }

    private UrlMetadata(string? createdBy, DateTime? expirationDate)
    {
        CreatedBy = createdBy;
        ExpirationDate = expirationDate;
    }

    public static UrlMetadata Create(string? createdBy = null, DateTime? expirationDate = null)
        => new(createdBy, expirationDate);

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CreatedBy;
        yield return ExpirationDate;
    }
}
