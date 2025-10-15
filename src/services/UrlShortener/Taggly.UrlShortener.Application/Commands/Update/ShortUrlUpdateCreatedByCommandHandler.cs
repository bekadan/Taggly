using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;
using Taggly.UrlShortener.Domain;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Commands.Update;

public sealed class ShortUrlUpdateCreatedByCommandHandler : ICommandHandler<ShortUrlUpdateCreatedByCommand, Result<ShortUrlResponse>>
{
    private readonly IAsyncRepository<ShortUrl> _shortUrlRepository;

    public ShortUrlUpdateCreatedByCommandHandler(IAsyncRepository<ShortUrl> shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<Result<ShortUrlResponse>> Handle(ShortUrlUpdateCreatedByCommand request, CancellationToken cancellationToken)
    {
        ShortUrl? shortUrl = await _shortUrlRepository.GetAsync(su=>su.Id == request.ShortUrlId, cancellationToken:cancellationToken, withDeleted:true);

        if (shortUrl is null || shortUrl.IsDeleted)
        {
            return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlNotFound);
        }

        shortUrl.Metadata.UpdateCreatedBy(request.UserId);

        await _shortUrlRepository.UpdateAsync(shortUrl, cancellationToken);

        ShortUrlResponse response = new(shortUrl);

        return Result<ShortUrlResponse, object>.Success(response, response);
    }
}
