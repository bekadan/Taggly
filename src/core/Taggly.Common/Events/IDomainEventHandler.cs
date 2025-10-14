using Taggly.Common.Abstractions;

namespace Taggly.Common.Events;

/// <summary>
/// Domain event handler contract.
/// </summary>
/// <typeparam name="TEvent">Type of domain event to handle.</typeparam>
public interface IDomainEventHandler<TEvent>
    where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
