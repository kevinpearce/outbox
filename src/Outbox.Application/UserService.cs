using Microsoft.Extensions.Logging;
using Outbox.Application.Interfaces;
using Outbox.Domain;

namespace Outbox.Application;

public class UserService(ILogger<UserService> logger, IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserService
{
    private const string UserCreatedMessage = "User {UserId} created";
    
    public async Task<Guid> CreateUserAsync(string name, CancellationToken cancellationToken = default)
    {
        var user = User.Create(name);

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation(UserCreatedMessage, user.Id);

        return user.Id;
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await userRepository.GetByIdAsync(id, cancellationToken);
}