using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Outbox.Domain;
using Outbox.Domain.Interfaces;

namespace Outbox.Infrastructure;

public class OutboxInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            InsertOutboxMessages(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void InsertOutboxMessages(DbContext context)
    {
        var outboxMessages = context.ChangeTracker
            .Entries<IEntity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = [..entity.DomainEvents];
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage(
                domainEvent.Id,
                domainEvent.GetType().FullName!,
                JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                domainEvent.OccurredOnUtc))
            .ToList();

        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}