using Outbox.Application;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.Outbox;

public static class ServiceRegistration
{
    public static IServiceCollection AddOutboxServices(this IServiceCollection services)
    {
        services.AddScoped<IOutboxService, OutboxService>();

        return services;
    }
}