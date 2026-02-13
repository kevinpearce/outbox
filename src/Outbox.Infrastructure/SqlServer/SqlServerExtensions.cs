using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Outbox.Infrastructure.SqlServer;

public static class SqlServerExtensions
{
    public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<OutboxInterceptor>());
        });

        return services;
    }
}
