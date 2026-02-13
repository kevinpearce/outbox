using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Outbox.Application.Interfaces;
using Outbox.Service.Models;

namespace Outbox.Service;

public class Worker(
    ILogger<Worker> logger,
    IServiceScopeFactory serviceScopeFactory,
    IOptions<UiOptions> uiOptions,
    IOptions<WorkerOptions> workerOptions
) : BackgroundService
{
    private readonly UiOptions _uiOptions = uiOptions.Value;
    private readonly WorkerOptions _workerOptions = workerOptions.Value;

    private HubConnection? _hubConnection;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Outbox Worker Service starting...");

        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_uiOptions.BaseUrl}{_uiOptions.OutboxHubPath}")
            .WithAutomaticReconnect()
            .Build();

        var pollingInterval = TimeSpan.FromSeconds(_workerOptions.PollingIntervalSeconds);

        while (!cancellationToken.IsCancellationRequested)
        {
            if (_hubConnection is { State: HubConnectionState.Disconnected })
            {
                try
                {
                    await _hubConnection.StartAsync(cancellationToken);

                    if (logger.IsEnabled(LogLevel.Information))
                        logger.LogInformation("Connected to SignalR hub at {url}", _uiOptions.BaseUrl);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Could not connect to SignalR hub. Will retry on next cycle.");
                }
            }
            try
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Checking for unprocessed outbox messages at: {time}", DateTimeOffset.Now);

                using var scope = serviceScopeFactory.CreateScope();
                var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();

                await outboxService.ProcessOutboxMessagesAsync(cancellationToken);

                if (_hubConnection?.State == HubConnectionState.Connected)
                {
                    await _hubConnection.InvokeAsync("NotifyMessageProcessed", cancellationToken);
                    logger.LogInformation("Sent SignalR notification");
                }

                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Finished processing outbox messages. Next check in {interval} seconds", pollingInterval.TotalSeconds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing outbox messages");
            }

            await Task.Delay(pollingInterval, cancellationToken);
        }

        logger.LogInformation("Outbox Worker Service stopping...");

        if (_hubConnection != null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}