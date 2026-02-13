using Outbox.Domain;

namespace Outbox.Application.Interfaces;

public interface IOutboxService
{
    Task<IEnumerable<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken = default);
    Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default);
}