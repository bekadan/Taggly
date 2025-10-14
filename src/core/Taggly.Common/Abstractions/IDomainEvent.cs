namespace Taggly.Common.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
