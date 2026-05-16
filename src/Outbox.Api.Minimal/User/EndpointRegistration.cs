using Outbox.Api.Minimal.Middleware;
using Outbox.Api.Minimal.User.Endpoints;
using Outbox.Api.Minimal.User.Models;

namespace Outbox.Api.Minimal.User;

public static class EndpointRegistration
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userEndpoints = app.MapGroup("/users");

        userEndpoints.MapPost("/", CreateUser.Handle)
            .AddEndpointFilter<GlobalValidationFilter<CreateUserRequest>>();

        userEndpoints.MapGet("/{id:guid}", GetUserById.Handle);

        return app;
    }
}
