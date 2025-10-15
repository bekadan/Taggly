using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taggly.UrlShortener.API.Abstractions;
using Taggly.UrlShortener.Application.Commands.Update;

namespace Taggly.UrlShortener.API.Endpoints;

public class Put : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/short-urls/update-created-by", async ([FromBody] ShortUrlUpdateCreatedByCommand command, IMediator mediator) => 
        { 
            var response = await mediator.Send(command);
            return Results.Ok(response);
        }).WithTags("Short Urls");

        app.MapPut("/short-urls/update-description", async ([FromBody] ShortUrlUpdateDescriptionCommand command, IMediator mediator) =>
        {
            var response = await mediator.Send(command);
            return Results.Ok(response);
        }).WithTags("Short Urls");
    }
}
