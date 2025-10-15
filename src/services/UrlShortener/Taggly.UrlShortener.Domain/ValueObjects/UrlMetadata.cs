using Taggly.Common.Domain;
using Taggly.Common.Types;

namespace Taggly.UrlShortener.Domain.ValueObjects;

public sealed class UrlMetadata : ValueObject
{
    public Guid CreatedBy { get; private set; }
    public DateTime? ExpirationDate { get; private set; }
    public string? Description { get; private set; }

    private UrlMetadata(Guid createdBy, DateTime? expirationDate = null, string? description = null)
    {
        Description = description;
        CreatedBy = createdBy;
        ExpirationDate = expirationDate;
    }

    public static Result<UrlMetadata> Create(Guid createdBy, DateTime? expirationDate = null, string? description = null)
    {
        if (expirationDate.HasValue && expirationDate <= DateTime.UtcNow)
            return Result.Failure<UrlMetadata>(Errors.UrlMetadata.InvalidExpirationDate);

        return Result.Success(new UrlMetadata(createdBy, expirationDate, description));
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return CreatedBy;
        yield return ExpirationDate ?? DateTime.MinValue;
        yield return Description ?? string.Empty;
    }

    public void UpdateCreatedBy(Guid createdBy)
    {
        CreatedBy = createdBy;
    }

    public Result UpdateExpirationDate(DateTime? expirationDate)
    {
        if (expirationDate.HasValue && expirationDate <= DateTime.UtcNow)
            return Result.Failure(Errors.UrlMetadata.InvalidExpirationDate);
        ExpirationDate = expirationDate;
        return Result.Success();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }
}
