using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;

namespace Taggly.UrlShortener.Application.Queries.GetByShortCode;

public record GetOriginalUrlByShortCodeQuery(string ShortCode) : IQuery<Result<string?>>;
