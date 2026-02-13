using Outbox.Application.Interfaces;
using Outbox.Domain;

namespace Outbox.Application;

public class UserService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    public async Task<Guid> CreateUserAsync(string name, CancellationToken cancellationToken = default)
    {
        var user = User.Create(name);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await userRepository.GetByIdAsync(id, cancellationToken);
}