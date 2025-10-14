using Microsoft.AspNetCore.Builder;
using Taggly.Exceptions.Middleware;

namespace Taggly.Exceptions.Extensions;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseTagglyExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

/*
 builder.Services.Configure<ExceptionHandlingOptions>(builder.Configuration.GetSection("ExceptionHandling"));

var app = builder.Build();

app.UseTagglyExceptionHandling();
 
 */