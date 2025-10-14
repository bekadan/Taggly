using Taggly.SharedKernel.Abstractions;

namespace Taggly.SharedKernel.Events;

/// <summary>
/// Simple dispatcher that resolves all registered handlers for the event type from <see cref="IServiceProvider"/>.
/// Handlers should be registered as <code>IDomainEventHandler{TEvent}</code>.
/// </summary>
public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent == null) throw new ArgumentNullException(nameof(domainEvent));

        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = (IEnumerable<object>?)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType))
                       ?? Enumerable.Empty<object>();

        var tasks = new List<Task>();

        foreach (var handler in handlers)
        {
            // Use dynamic invocation to call HandleAsync. This keeps the dispatcher free of compile-time coupling.
            var handleMethod = handler.GetType().GetMethod("HandleAsync");
            if (handleMethod == null) continue;

            var result = handleMethod.Invoke(handler, new object[] { domainEvent, cancellationToken });
            if (result is Task t) tasks.Add(t);
        }

        if (tasks.Count > 0)
            await Task.WhenAll(tasks).ConfigureAwait(false);
    }
}
