using Microsoft.AspNetCore.Http.HttpResults;
using Outbox.Api.Minimal.User.Models;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.User.Endpoints;

public static class CreateUser
{
    public static async Task<Results<Created<CreatedUserResponse>, ValidationProblem>> Handle(
        CreateUserRequest request, IUserService userService, CancellationToken cancellationToken)
    {
        var userId = await userService.CreateUserAsync(request.Name, cancellationToken);

        return TypedResults.Created($"/users/{userId}", new CreatedUserResponse(userId));
    }
}