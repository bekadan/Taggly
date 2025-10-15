using Taggly.UrlShortener.Domain.Entities;

namespace Taggly.UrlShortener.Application.Contracts;

public sealed record ShortUrlResponse(Guid? UserId, ShortUrl ShortUrl);
