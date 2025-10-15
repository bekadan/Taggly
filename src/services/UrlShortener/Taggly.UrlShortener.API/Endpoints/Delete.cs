using Taggly.UrlShortener.API.Abstractions;

namespace Taggly.UrlShortener.API.Endpoints;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/", () => { });
    }
}
