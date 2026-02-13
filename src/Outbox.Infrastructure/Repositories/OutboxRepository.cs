using Microsoft.EntityFrameworkCore;
using Outbox.Application.Interfaces;
using Outbox.Domain;

namespace Outbox.Infrastructure.Repositories;

public sealed class OutboxRepository(AppDbContext context) : IOutboxRepository
{
    private readonly DbSet<OutboxMessage> _dbSet = context.OutboxMessages;

    public async Task<IEnumerable<OutboxMessage>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.ToListAsync(cancellationToken);

    public async Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync(CancellationToken cancellationToken = default) =>
        await _dbSet
            .Where(m => m.ProcessedOnUtc == null && m.Error == null)
            .OrderBy(m => m.OccurredOnUtc)
            .ToListAsync(cancellationToken);

    public async Task MarkAsProcessedAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await context.OutboxMessages
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.ProcessedOnUtc, DateTime.UtcNow), cancellationToken);
    }

    public async Task MarkAsFailedAsync(Guid id, string error, CancellationToken cancellationToken = default)
    {
        await context.OutboxMessages
            .Where(m => m.Id == id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Error, error), cancellationToken);
    }
}