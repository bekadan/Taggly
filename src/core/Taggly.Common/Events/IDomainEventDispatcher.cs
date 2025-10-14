using Taggly.Common.Abstractions;

namespace Taggly.Common.Events;

/// <summary>
/// Dispatches domain events to matching handlers resolved from DI.
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
