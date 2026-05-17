using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

internal sealed class ThrowingUserService : IUserService
{
    private const string SimulatedExceptionMessage = "Simulated unhandled exception";

    public Task<Guid> CreateUserAsync(string name, CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(SimulatedExceptionMessage);

    public Task<global::Outbox.Domain.User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        throw new InvalidOperationException(SimulatedExceptionMessage);
}
