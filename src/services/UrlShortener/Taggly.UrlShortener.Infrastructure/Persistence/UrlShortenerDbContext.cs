using Microsoft.EntityFrameworkCore;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Infrastructure.Persistence
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options)
            : base(options)
        {
        }

        public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
