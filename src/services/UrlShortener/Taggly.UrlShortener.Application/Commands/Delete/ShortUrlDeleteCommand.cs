using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;

namespace Taggly.UrlShortener.Application.Commands.Delete;

public record ShortUrlDeleteCommand(Guid UserId, string ShortCode) : ICommand<Result<ShortUrlResponse>>;
