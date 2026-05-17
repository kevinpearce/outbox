namespace Outbox.Api.Minimal.Tests.Integration.Outbox;

public record OutboxMessagesResponse(List<OutboxMessageItem> Messages);