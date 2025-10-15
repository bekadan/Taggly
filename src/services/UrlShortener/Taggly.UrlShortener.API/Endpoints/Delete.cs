using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taggly.UrlShortener.API.Abstractions;
using Taggly.UrlShortener.Application.Commands.Delete;

namespace Taggly.UrlShortener.API.Endpoints;

public class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/short-urls/delete", async ([FromBody] ShortUrlDeleteCommand command, IMediator mediator) => 
        { 
            var response = await mediator.Send(command);
            return Results.Ok(response);
        }).WithTags("Short Urls");
    }
}
