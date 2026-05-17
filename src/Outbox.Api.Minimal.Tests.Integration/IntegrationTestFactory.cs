using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MsSql;

namespace Outbox.Api.Minimal.Tests.Integration;

public class IntegrationTestFactory : WebApplicationFactory<Program>
{
    private const string SqlServerImage = "mcr.microsoft.com/mssql/server:2022-latest";
    private const string ConnectionStringKey = "ConnectionStrings:DefaultConnection";

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder(SqlServerImage)
        .Build();

    public async Task InitialiseAsync()
    {
        await _dbContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [ConnectionStringKey] = _dbContainer.GetConnectionString()
            });
        });
    }

    public async Task DisposeContainerAsync()
    {
        await _dbContainer.StopAsync();
        await _dbContainer.DisposeAsync();
    }
}
