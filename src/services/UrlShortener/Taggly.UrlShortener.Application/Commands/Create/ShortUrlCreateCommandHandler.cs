using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Abstractions.Repositories;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;
using Taggly.UrlShortener.Application.Interfaces;
using Taggly.UrlShortener.Domain;
using Taggly.UrlShortener.Domain.Entities;
using Taggly.UrlShortener.Domain.ValueObjects;

namespace Taggly.UrlShortener.Application.Commands.Create
{
    public sealed class ShortUrlCreateCommandHandler : ICommandHandler<ShortUrlCreateCommand, Result<ShortUrlResponse>>
    {
        private readonly IAsyncRepository<ShortUrl> _shortUrlRepository;
        private readonly IShortCodeGenerator _shortCodeGenerator;

        public ShortUrlCreateCommandHandler(IAsyncRepository<ShortUrl> shortUrlRepository, IShortCodeGenerator shortCodeGenerator)
        {
            _shortUrlRepository = shortUrlRepository;
            _shortCodeGenerator = shortCodeGenerator;
        }

        public async Task<Result<ShortUrlResponse>> Handle(ShortUrlCreateCommand request, CancellationToken cancellationToken)
        {
            var originalUrlResult = OriginalUrl.Create(request.OriginalUrl);

            if (originalUrlResult.IsFailure)
            {
                return Result.Failure<ShortUrlResponse>(originalUrlResult.Error);
            }

            var metadataResult = UrlMetadata.Create(request.UserId, request.ExpirationDate, request.Description);

            if (metadataResult.IsFailure)
            {
                return Result.Failure<ShortUrlResponse>(metadataResult.Error);
            }

            string shortCodeValue = string.Empty;
            int retryCount = 0;

            do
            {
                if (retryCount++ > 10)
                {
                    return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlCouldntCreated);
                }

                shortCodeValue = _shortCodeGenerator.Generate(3);

            } while (await _shortUrlRepository.AnyAsync(predicate: s => s.ShortCode.Value.Equals(shortCodeValue), cancellationToken: cancellationToken, withDeleted: true));

            if (string.IsNullOrEmpty(shortCodeValue))
            {
                return Result.Failure<ShortUrlResponse>(Errors.ShortUrl.ShortUrlCouldntCreated);
            }

            var shortCodeResult = ShortCode.Create(shortCodeValue);

            if (shortCodeResult.IsFailure)
            {   
                return Result.Failure<ShortUrlResponse>(shortCodeResult.Error);
            }

            var shortUrlCreateResult = ShortUrl.Create(originalUrlResult.Value(), shortCodeResult.Value(), metadataResult.Value());

            await _shortUrlRepository.AddAsync(shortUrlCreateResult, cancellationToken);

            var response = new ShortUrlResponse(shortUrlCreateResult);

            return Result<ShortUrlResponse, object>.Success(response, response);
        }
    }
}
