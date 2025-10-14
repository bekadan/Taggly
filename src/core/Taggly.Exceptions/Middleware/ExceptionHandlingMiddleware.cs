using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using Taggly.Exceptions.Base;
using Taggly.Exceptions.Options;
using Taggly.Logging.Abstractions;

namespace Taggly.Exceptions.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITagglyLogger _logger;
    private readonly ExceptionHandlingOptions _options;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ITagglyLogger logger,
        IOptions<ExceptionHandlingOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (TagglyException tex)
        {
            _logger.LogWarning("Handled Taggly exception", tex, tex.Message);
            await HandleExceptionAsync(context, tex, GetStatusCode(tex));
        }
        catch (Exception ex)
        {
            _logger.LogError("Unhandled exception", ex, ex.Message);
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private static HttpStatusCode GetStatusCode(TagglyException ex) =>
        ex switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            NotFoundException => HttpStatusCode.NotFound,
            UnauthorizedException => HttpStatusCode.Unauthorized,
            ConflictException => HttpStatusCode.Conflict,
            InfrastructureException => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var isDev = _options.IncludeDetailsInDevelopment &&
                    context.RequestServices.GetService<IHostEnvironment>()?.IsDevelopment() == true;

        var problemDetails = new TagglyProblemDetails
        {
            Type = $"https://httpstatuses.com/{(int)statusCode}",
            Title = ex is TagglyException tex ? tex.GetType().Name : "InternalServerError",
            Status = (int)statusCode,
            Detail = isDev ? ex.ToString() : ex.Message,
            Instance = context.TraceIdentifier
        };

        if (ex is ValidationException vex)
        {
            problemDetails.Errors = vex.Errors.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
        }

        var json = JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
