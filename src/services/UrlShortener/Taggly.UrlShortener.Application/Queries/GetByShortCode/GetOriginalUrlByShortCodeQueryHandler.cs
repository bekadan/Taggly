using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Interfaces.Repositories;

namespace Taggly.UrlShortener.Application.Queries.GetByShortCode;

public class GetOriginalUrlByShortCodeQueryHandler : IQueryHandler<GetOriginalUrlByShortCodeQuery, Result<string?>>
{
    private readonly IShortUrlRepository _repository;

    public GetOriginalUrlByShortCodeQueryHandler(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<string?>> Handle(GetOriginalUrlByShortCodeQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByShortCodeAsync(request.ShortCode, cancellationToken);
        if (entity == null) return null;

        // Optionally, check expiration
        if (entity.ExpirationDate.HasValue && entity.ExpirationDate <= System.DateTime.UtcNow)
            return null;

        return entity.OriginalUrl.Value;
    }
}
