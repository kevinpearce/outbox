using Microsoft.AspNetCore.Diagnostics;

namespace Outbox.Api.Minimal.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string ProblemDetailsContentType = "application/problem+json";
    private const string GenericErrorTitle = "An error occurred while processing your request";
    private const string LogMessageTemplate = "An unhandled exception occurred: {Message}";
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, LogMessageTemplate, exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = ProblemDetailsContentType;

        var problemDetails = new
        {
            title = GenericErrorTitle,
            status = StatusCodes.Status500InternalServerError,
            detail = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
