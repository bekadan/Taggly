using Taggly.SharedKernel.Abstractions;

namespace Taggly.SharedKernel.Domain.Events;

/// <summary>
/// Base domain event providing occured timestamp.
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; }

    protected DomainEventBase()
    {
        OccurredOn = DateTime.UtcNow;
    }
}
