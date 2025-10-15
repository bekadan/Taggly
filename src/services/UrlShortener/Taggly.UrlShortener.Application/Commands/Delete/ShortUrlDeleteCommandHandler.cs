using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;
using Taggly.UrlShortener.Domain;
using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Commands.Delete;

public sealed class ShortUrlDeleteCommandHandler : ICommandHandler<ShortUrlDeleteCommand, Result<ShortUrlResponse>>
{
    private readonly IAsyncRepository<ShortUrl> _shortUrlRepository;

    public ShortUrlDeleteCommandHandler(IAsyncRepository<ShortUrl> shortUrlRepository)
    {
        _shortUrlRepository = shortUrlRepository;
    }

    public async Task<Result<ShortUrlResponse>> Handle(ShortUrlDeleteCommand request, CancellationToken cancellationToken)
    {
        ShortUrl? shortUrl = await _shortUrlRepository.GetAsync(
            su => su.ShortCode.Value == request.ShortCode && su.Metadata.CreatedBy == request.UserId, 
            cancellationToken:cancellationToken);

        if (shortUrl is null)
        {
            return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlNotFound);
        }

        await _shortUrlRepository.DeleteAsync(shortUrl, false, cancellationToken);

        var response = new ShortUrlResponse(shortUrl);

        return Result<ShortUrlResponse, object>.Success(response, response);
    }
}
