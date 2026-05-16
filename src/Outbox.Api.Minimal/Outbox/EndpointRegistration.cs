using Outbox.Api.Minimal.Outbox.Endpoints;

namespace Outbox.Api.Minimal.Outbox;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapOutboxEndpoints(this IEndpointRouteBuilder app)
    {
        var outboxEndpoints = app.MapGroup("/outbox");
        
        outboxEndpoints.MapGet("/", GetOutboxMessages.Handle);

        return app;
    }
}