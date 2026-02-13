using Microsoft.EntityFrameworkCore;
using Outbox.Application.Interfaces;
using Outbox.Domain;

namespace Outbox.Infrastructure.Repositories;

public sealed class UserRepository(AppDbContext context) : IUserRepository
{
    private readonly DbSet<User> _dbSet = context.Users;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await _dbSet.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default) => 
        await _dbSet.ToListAsync(cancellationToken);

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(user, cancellationToken);
    }
}