using Microsoft.Extensions.DependencyInjection;

namespace Taggly.SharedKernel.Events;

/// <summary>
/// DI registration helpers for domain event components.
/// </summary>
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTagglyDomainEvents(this IServiceCollection services)
    {
        services.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
        // Note: handlers are registered in application projects, typically as transient or scoped:
        // services.AddScoped<IDomainEventHandler<SomeEvent>, SomeEventHandler>();
        return services;
    }
}
