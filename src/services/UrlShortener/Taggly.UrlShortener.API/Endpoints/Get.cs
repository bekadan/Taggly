using MediatR;
using Taggly.UrlShortener.API.Abstractions;
using Taggly.UrlShortener.Application.Queries.GetByShortCode;

namespace Taggly.UrlShortener.API.Endpoints;

public class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/short-urls/get-by-short-code/{code}", async (string code, IMediator mediator) => 
        { 
            var query = new GetUrlByShortCodeQuery(code);
            var response = await mediator.Send(query);
            return Results.Ok(response);
        }).WithTags("Short Urls");
    }
}
