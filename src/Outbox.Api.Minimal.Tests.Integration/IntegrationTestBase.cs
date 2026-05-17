using System.Text.Json;

namespace Outbox.Api.Minimal.Tests.Integration;

public abstract class IntegrationTestBase
{
    protected static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    protected static HttpClient Client => TestSetup.Client;
}
