namespace Outbox.Api.Minimal.Tests.Integration.Outbox;

// ReSharper disable once ClassNeverInstantiated.Global
public record OutboxMessageItem(Guid Id, string Type, string Content, DateTime OccurredOnUtc);