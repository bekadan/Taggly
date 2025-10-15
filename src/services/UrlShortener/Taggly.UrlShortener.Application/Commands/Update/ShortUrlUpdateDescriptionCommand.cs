using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;

namespace Taggly.UrlShortener.Application.Commands.Update;

public sealed record ShortUrlUpdateDescriptionCommand(Guid ShortUrlId, string? Description) : ICommand<Result<ShortUrlResponse>>;
