using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.DTOs;

namespace Taggly.UrlShortener.Application.Commands.Create
{
    public record CreateShortUrlCommand(
        string OriginalUrl,
        DateTime? ExpirationDate = null,
        string? Description = null
    ) : ICommand<Result<ShortUrlDto>>; 
}
