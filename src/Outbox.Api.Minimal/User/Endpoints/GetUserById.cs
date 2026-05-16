using Microsoft.AspNetCore.Http.HttpResults;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.User.Endpoints;

public static class GetUserById
{
    public static async Task<Results<Ok<Domain.User>, NotFound>> Handle(
        Guid id, IUserService userService, CancellationToken cancellationToken)
    {
        var user = await userService.GetUserByIdAsync(id, cancellationToken);
        return user == null ? TypedResults.NotFound() : TypedResults.Ok(user);
    }
}
