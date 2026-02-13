using Outbox.Application.Interfaces;
using Outbox.Domain;

namespace Outbox.Application;

public class OutboxService(IOutboxRepository outboxRepository) : IOutboxService
{
    public async Task<IEnumerable<OutboxMessage>> GetOutboxMessagesAsync(CancellationToken cancellationToken = default) => 
        await outboxRepository.GetAllAsync(cancellationToken);

    public async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default)
    {
        var messages = await outboxRepository.GetUnprocessedAsync(cancellationToken);

        foreach (var message in messages)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                // Simulating processing of an outbox message - do actual work here
                await Task.Delay(100, cancellationToken);

                await outboxRepository.MarkAsProcessedAsync(message.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                await outboxRepository.MarkAsFailedAsync(message.Id, ex.Message, cancellationToken);
            }
        }
    }
}