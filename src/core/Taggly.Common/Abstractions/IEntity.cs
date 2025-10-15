namespace Taggly.Common.Abstractions;

public interface IEntity
{
    Guid Id { get; }

    DateTime CreatedOnUtc { get; }
    DateTime? ModifiedOnUtc { get; }
    DateTime? DeletedOnUtc { get; }
    bool IsDeleted { get; }
}
