using Microsoft.AspNetCore.Mvc;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Controllers;

[ApiController]
[Route("outbox")]
public class OutboxController(IOutboxService outboxService, ILogger<OutboxController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOutboxMessages(CancellationToken cancellationToken)
    {
        try
        {
            var messages = await outboxService.GetOutboxMessagesAsync(cancellationToken);
            return Ok(new { Messages = messages });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get outbox messages");
            return StatusCode(500, new { error = "An error occurred while retrieving outbox messages" });
        }
    }
}