using Taggly.Common.Abstractions.Repositories;
using Taggly.UrlShortener.Domain.Entities;
using Taggly.UrlShortener.Infrastructure.Persistence;

namespace Taggly.UrlShortener.Infrastructure.Repositories;

public class ShortUrlRepository : AsyncRepositoryBase<ShortUrl, UrlShortenerDbContext>
{
    public ShortUrlRepository(UrlShortenerDbContext context) : base(context)
    {
    }
}
