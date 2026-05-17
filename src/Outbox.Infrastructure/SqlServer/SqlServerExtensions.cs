using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Outbox.Infrastructure.SqlServer;

public static class SqlServerExtensions
{
    public static IServiceCollection AddSqlServer(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            
            options.UseSqlServer(connectionString)
                .AddInterceptors(sp.GetRequiredService<OutboxInterceptor>());
        });

        return services;
    }
}
