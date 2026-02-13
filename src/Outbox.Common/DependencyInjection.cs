using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Outbox.Common;

public static class DependencyInjection
{
    public static IServiceCollection AddLoggingService(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        return services.AddSerilog();
    }
}