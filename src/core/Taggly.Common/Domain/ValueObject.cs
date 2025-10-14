using Taggly.Common.Abstractions;

namespace Taggly.Common.Domain;

/// <summary>
/// Base class for value objects. Implements structural equality across atomic values.
/// </summary>
public abstract class ValueObject : IValueObject
{
    protected abstract IEnumerable<object?> GetAtomicValues();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType()) return false;

        var other = (ValueObject)obj;
        using var thisValues = GetAtomicValues().GetEnumerator();
        using var otherValues = other.GetAtomicValues().GetEnumerator();

        while (thisValues.MoveNext() && otherValues.MoveNext())
        {
            var a = thisValues.Current;
            var b = otherValues.Current;

            if (a == null ^ b == null) return false;
            if (a != null && !a.Equals(b)) return false;
        }

        return !thisValues.MoveNext() && !otherValues.MoveNext();
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            foreach (var obj in GetAtomicValues())
                hash = hash * 31 + (obj?.GetHashCode() ?? 0);
            return hash;
        }
    }

    public static bool operator ==(ValueObject? a, ValueObject? b)
        => a is null ? b is null : a.Equals(b);

    public static bool operator !=(ValueObject? a, ValueObject? b)
        => !(a == b);
}
