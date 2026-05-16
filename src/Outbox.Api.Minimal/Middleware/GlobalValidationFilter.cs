using FluentValidation;

namespace Outbox.Api.Minimal.Middleware;

public class GlobalValidationFilter<TRequest>(ILogger<GlobalValidationFilter<TRequest>> logger) : IEndpointFilter
    where TRequest : class
{
    private const string MissingRequestExceptionMessage = "Request of type {0} not found in endpoint arguments.";
    private const string ValidationFailedMessage = "Validation failed for {RequestType}: {Errors}";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var (validator, request) = GetValidatorAndRequest(context);
        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);
        if (validationResult.IsValid) return await next(context);

        logger.LogWarning(ValidationFailedMessage, typeof(TRequest).Name, validationResult.Errors);
        return TypedResults.ValidationProblem(BuildValidationErrors(validationResult));
    }

    private static (IValidator<TRequest> validator, TRequest request) GetValidatorAndRequest(EndpointFilterInvocationContext context)
    {
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<TRequest>>();

        return context.Arguments.FirstOrDefault(a => a is TRequest) is not TRequest request 
            ? throw new InvalidOperationException(string.Format(MissingRequestExceptionMessage, typeof(TRequest).Name)) 
            : (validator, request);
    }

    private static Dictionary<string, string[]> BuildValidationErrors(FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}
