using Outbox.Domain;

namespace Outbox.Application.Interfaces;

public interface IOutboxRepository
{
    Task<IEnumerable<OutboxMessage>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<OutboxMessage>> GetUnprocessedAsync(CancellationToken cancellationToken = default);
    Task MarkAsProcessedAsync(Guid id, CancellationToken cancellationToken = default);
    Task MarkAsFailedAsync(Guid id, string error, CancellationToken cancellationToken = default);
}