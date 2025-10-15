using Taggly.Common.Abstractions.Repositories;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Interfaces.Repositories;

public interface IShortUrlRepository : IAsyncRepository<ShortUrl, Guid>
{
    Task<ShortUrl?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);
    Task<bool> ShortCodeExistsAsync(string shortCode, CancellationToken cancellationToken = default);
}
