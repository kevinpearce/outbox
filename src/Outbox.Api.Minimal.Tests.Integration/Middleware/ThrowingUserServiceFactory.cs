using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Outbox.Application.Interfaces;

namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

internal sealed class ThrowingUserServiceFactory : IntegrationTestFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.Single(d => d.ServiceType == typeof(IUserService));
            services.Remove(descriptor);
            services.AddScoped<IUserService, ThrowingUserService>();
        });
    }
}
