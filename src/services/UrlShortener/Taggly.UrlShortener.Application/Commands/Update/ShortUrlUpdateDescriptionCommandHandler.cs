using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;
using Taggly.UrlShortener.Domain;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Commands.Update;

public sealed class ShortUrlUpdateDescriptionCommandHandler : ICommandHandler<ShortUrlUpdateDescriptionCommand, Result<ShortUrlResponse>>
{
    private readonly IAsyncRepository<ShortUrl> _shortUrlRepository;

    public ShortUrlUpdateDescriptionCommandHandler(IAsyncRepository<ShortUrl> shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<Result<ShortUrlResponse>> Handle(ShortUrlUpdateDescriptionCommand request, CancellationToken cancellationToken)
    {
        ShortUrl? shortUrl = await _shortUrlRepository.GetAsync(su=>su.Id == request.ShortUrlId, cancellationToken:cancellationToken);

        if (shortUrl is null)
        {
            return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlNotFound);
        }

        shortUrl.Metadata.UpdateDescription(request.Description);

        await _shortUrlRepository.UpdateAsync(shortUrl, cancellationToken);

        var response = new ShortUrlResponse(shortUrl);

        return Result<ShortUrlResponse, object>.Success(response, response);
    }
}
