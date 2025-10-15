using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;
using Taggly.UrlShortener.Domain;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Queries.GetByShortCode;

public sealed class GetUrlByShortCodeQueryHandler : IQueryHandler<GetUrlByShortCodeQuery, Result<ShortUrlResponse>>
{
    private readonly IAsyncRepository<ShortUrl> _shortUrlRepository;

    public GetUrlByShortCodeQueryHandler(IAsyncRepository<ShortUrl> shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<Result<ShortUrlResponse>> Handle(GetUrlByShortCodeQuery request, CancellationToken cancellationToken)
    {
        ShortUrl? shortUrl = await _shortUrlRepository.GetAsync(su => su.ShortCode.Value == request.ShortCode, cancellationToken:cancellationToken);

        if (shortUrl is null)
        {
            return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlNotFound);
        }

        if (shortUrl.Metadata.ExpirationDate is not null && shortUrl.Metadata.ExpirationDate <= DateTime.UtcNow)
        {
            return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlExpired);
        }

        ShortUrlResponse response = new(shortUrl);

        return Result<ShortUrlResponse, object>.Success(response, response);
    }
}
