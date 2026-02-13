using Outbox.Domain.Interfaces;

namespace Outbox.Domain;

public sealed record UserCreatedEvent(Guid UserId, string Name) : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
