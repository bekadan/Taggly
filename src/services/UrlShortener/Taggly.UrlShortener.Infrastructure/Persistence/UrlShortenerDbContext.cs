using Microsoft.EntityFrameworkCore;
using Taggly.Common.Domain;
using Taggly.UrlShortener.Domain.Entities;
using Taggly.UrlShortener.Infrastructure.Configurations;

namespace Taggly.UrlShortener.Infrastructure.Persistence
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext()
        {
            
        }

        public UrlShortenerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UrlShortenerDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch domain events before saving
            var domainEntities = ChangeTracker.Entries<AggregateRoot>();

            foreach (var entry in domainEntities)
            {
                var entity = entry.Entity;
                foreach (var domainEvent in entity.DomainEvents)
                {
                    // Here you can send events to a bus or handle them synchronously
                    // Example: await _domainEventDispatcher.Dispatch(domainEvent);
                    // For now, we just log to console
                    Console.WriteLine($"Domain Event: {domainEvent.GetType().Name}");
                }

                entity.ClearDomainEvents(); // Clear after dispatching
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
