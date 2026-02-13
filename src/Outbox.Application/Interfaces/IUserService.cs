using Outbox.Domain;

namespace Outbox.Application.Interfaces;

public interface IUserService
{
    Task<Guid> CreateUserAsync(string name, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);
}