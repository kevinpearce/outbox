using Microsoft.AspNetCore.SignalR;

namespace Outbox.Ui.Hubs;

public class OutboxHub : Hub
{
    public async Task NotifyMessageProcessed() => await Clients.All.SendAsync("MessageProcessed");
}
