using MediatR;
using Taggly.UrlShortener.API.Abstractions;
using Taggly.UrlShortener.Application.Commands.Create;

namespace Taggly.UrlShortener.API.Endpoints;

public class Post : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/short-urls/create", async (ShortUrlCreateCommand shortUrlCreate, IMediator mediator) => 
        {
            var response = await mediator.Send(shortUrlCreate);
            return Results.Ok(response);
        }).WithTags("Short Urls");
    }
}
