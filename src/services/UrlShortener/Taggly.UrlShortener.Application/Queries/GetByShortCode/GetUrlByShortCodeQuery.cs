using Taggly.Common.Abstractions.Messaging;
using Taggly.Common.Types;
using Taggly.UrlShortener.Application.Contracts;

namespace Taggly.UrlShortener.Application.Queries.GetByShortCode
{
    public sealed class GetUrlByShortCodeQuery : IQuery<Result<ShortUrlResponse>>
    {
        public string ShortCode { get; set; }

        public GetUrlByShortCodeQuery(string shortCode)
        {
            ShortCode = shortCode;
        }
    }
}
