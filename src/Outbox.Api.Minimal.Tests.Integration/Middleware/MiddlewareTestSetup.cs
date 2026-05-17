namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

[SetUpFixture]
public class MiddlewareTestSetup
{
    private static ThrowingUserServiceFactory Factory { get; set; } = null!;
    public static HttpClient Client { get; private set; } = null!;

    [OneTimeSetUp]
    public async Task GlobalSetUp()
    {
        Factory = new ThrowingUserServiceFactory();
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
