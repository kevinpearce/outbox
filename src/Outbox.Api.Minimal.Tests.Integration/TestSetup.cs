namespace Outbox.Api.Minimal.Tests.Integration;

[SetUpFixture]
public class TestSetup
{
    private static IntegrationTestFactory Factory { get; set; } = null!;
    public static HttpClient Client { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task GlobalSetUp()
    {
        Factory = new IntegrationTestFactory();
        await Factory.InitialiseAsync();
        Client = Factory.CreateClient();
    }

    [OneTimeTearDown]
    public async Task GlobalTearDown()
    {
        Client.Dispose();
        await Factory.DisposeContainerAsync();
        await Factory.DisposeAsync();
    }
}
