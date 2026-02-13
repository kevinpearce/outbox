namespace Outbox.Domain;

public record OutboxMessage(Guid Id, string Type, string Content, DateTime OccurredOnUtc)
{
    public DateTime? ProcessedOnUtc { get; init; }
    public string? Error { get; init; }
}