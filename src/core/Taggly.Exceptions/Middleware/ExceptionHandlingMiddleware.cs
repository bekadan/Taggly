using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
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
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var isDev = _options.IncludeDetailsInDevelopment &&
                    context.RequestServices.GetService(typeof(IHostEnvironment)) is IHostEnvironment env &&
                    env.IsDevelopment();

        var response = new
        {
            error = ex.Message,
            type = ex.GetType().Name,
            correlationId = context.TraceIdentifier,
            details = isDev ? ex.StackTrace : null,
            validationErrors = ex is ValidationException vex ? vex.Errors : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
