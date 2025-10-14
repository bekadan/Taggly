using Taggly.SharedKernel.Abstractions;

namespace Taggly.SharedKernel.Domain;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; protected set; } = default!;

    protected BaseEntity() { }

    protected BaseEntity(Guid id) => Id = id;

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<Guid>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
        => object.Equals(a, b);

    public static bool operator !=(BaseEntity? a, BaseEntity? b)
        => !(a == b);
}
