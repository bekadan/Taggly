using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.DTOs;
using Taggly.UrlShortener.Application.Interfaces;
using Taggly.UrlShortener.Application.Interfaces.Repositories;
using Taggly.UrlShortener.Domain.Entities;
using Taggly.UrlShortener.Domain.ValueObjects;

namespace Taggly.UrlShortener.Application.Commands.Create
{
    public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, Result<ShortUrlDto>>
    {
        private readonly IShortUrlRepository _repository;
        private readonly IShortCodeGenerator _shortCodeGenerator;

        public CreateShortUrlCommandHandler(IShortUrlRepository repository, IShortCodeGenerator shortCodeGenerator)
        {
            _repository = repository;
            _shortCodeGenerator = shortCodeGenerator;
        }

        public async Task<Result<ShortUrlDto>> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
        {
            var originalUrl = OriginalUrl.Create(request.OriginalUrl);
            var metadata = UrlMetadata.Create(null, request.ExpirationDate);

            // Generate a unique short code
            string shortCodeValue;
            int retryCount = 0;
            do
            {
                if (retryCount++ > 10)
                    throw new InvalidOperationException("Unable to generate unique short code after 5 attempts.");

                shortCodeValue = _shortCodeGenerator.Generate();
            }
            while (await _repository.ShortCodeExistsAsync(shortCodeValue, cancellationToken));

            var shortCode = ShortCode.Create(shortCodeValue);

            var shortUrl = ShortUrl.Create(originalUrl, shortCode, metadata);

            await _repository.AddAsync(shortUrl, cancellationToken);

            return new ShortUrlDto
            {
                ShortCode = shortUrl.ShortCode.Value,
                OriginalUrl = shortUrl.OriginalUrl.Value,
                CreatedAt = shortUrl.CreatedAt,
                ExpirationDate = shortUrl.ExpirationDate
            };
        }
    }
}
