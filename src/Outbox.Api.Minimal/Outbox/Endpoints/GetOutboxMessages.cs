using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.Outbox.Endpoints;

public static class GetOutboxMessages
{
    public static async Task<IResult> Handle(
        IOutboxService outboxService, CancellationToken cancellationToken)
    {
        var messages = await outboxService.GetOutboxMessagesAsync(cancellationToken);
        return TypedResults.Ok(new { Messages = messages });
    }
}