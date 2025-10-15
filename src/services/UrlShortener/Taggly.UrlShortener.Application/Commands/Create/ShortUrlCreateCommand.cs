using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;

namespace Taggly.UrlShortener.Application.Commands.Create;

public sealed record ShortUrlCreateCommand(Guid? UserId, string OriginalUrl, DateTime? ExpirationDate, string? Description) : ICommand<Result<ShortUrlResponse>>;
