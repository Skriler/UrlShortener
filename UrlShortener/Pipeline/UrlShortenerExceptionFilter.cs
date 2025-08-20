using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UrlShortener.Exceptions;

namespace UrlShortener.Pipeline;

/// <summary>
/// Exception filter for handling business logic errors.
/// Logs the error and creates ProblemDetails with additional information for the response to the client.
/// </summary>
public class UrlShortenerExceptionFilter(
    ILogger<UrlShortenerExceptionFilter> logger,
    IHostEnvironment environment
    ) : IExceptionFilter
{
    /// <summary>
    /// Handles exceptions of type UrlShortenerException.
    /// Logs the error information and prepares a response with problem details.
    /// </summary>
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not UrlShortenerException ex)
            return;

        logger.LogWarning(
            ex,
            "Url Shortener exception occurred: {ErrorTitle} - {Message}",
            ex.ErrorTitle,
            ex.Message);

        var problemDetails = new ProblemDetails
        {
            Title = ex.ErrorTitle,
            Status = ex.StatusCode,
            Detail = ex.Message,
            Instance = context.HttpContext.Request.Path,
        };

        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        if (environment.IsDevelopment())
        {
            problemDetails.Extensions["stackTrace"] = ex.StackTrace;
        }

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = ex.StatusCode
        };

        context.ExceptionHandled = true;
    }
}

public static partial class ErrorHandlingExtensions
{
    /// <summary>
    /// Extension method for registering the UrlShortenerExceptionFilter.
    /// </summary>
    public static IMvcBuilder AddUrlShortenerExceptionFilter(this IMvcBuilder builder)
    {
        builder.AddMvcOptions(options => options.Filters.Add<UrlShortenerExceptionFilter>());

        return builder;
    }
}