using System.Text.Json;

namespace UrlShortener.Middlewares;

/// <summary>
/// Middleware for global error handling in the application.
/// Logs errors and sends a JSON response with details about the error to the client.
/// </summary>
public class ErrorHandlingMiddleware(
    RequestDelegate next,
    ILogger<ErrorHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    private const int InternalServerStatusCode = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// Processes the request and catches any exceptions that may occur during its processing.
    /// If an exception is thrown, it calls the method to handle and return the error response.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Exception occurred: {Message}", ex.Message);

            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Creates and sends a JSON response with details about the error.
    /// In development mode, includes the stack trace; otherwise, only a general error message is sent.
    /// </summary>
    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        string message = environment.IsDevelopment()
            ? ex.Message
            : "An internal server error has occurred.";

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = InternalServerStatusCode;

        var result = JsonSerializer.Serialize(new
        {
            status = InternalServerStatusCode,
            detail = message,
            instance = context.Request.Path.Value,
            timestamp = DateTime.UtcNow,
            stackTrace = environment.IsDevelopment() ? ex.ToString() : null,
        });

        await context.Response.WriteAsync(result);
    }
}

public static class ErrorHandlingExtensions
{
    /// <summary>
    /// Extension method for adding ErrorHandlingMiddleware to the application's request pipeline.
    /// </summary>
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}