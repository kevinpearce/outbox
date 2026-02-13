using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Outbox.Application.Interfaces;
using Outbox.Infrastructure.Repositories;
using Outbox.Infrastructure.SqlServer;

namespace Outbox.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<OutboxInterceptor>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSqlServer(configuration);

        return services;
    }

    public static async Task<IServiceProvider> UseInfrastructure(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // POC only, not for production use
        await dbContext.Database.EnsureCreatedAsync();

        return services;
    }
}