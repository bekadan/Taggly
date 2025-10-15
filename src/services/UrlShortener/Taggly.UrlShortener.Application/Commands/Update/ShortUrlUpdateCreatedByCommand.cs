using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;

namespace Taggly.UrlShortener.Application.Commands.Update;

public sealed record ShortUrlUpdateCreatedByCommand(Guid UserId, Guid ShortUrlId) : ICommand<Result<ShortUrlResponse>>;
