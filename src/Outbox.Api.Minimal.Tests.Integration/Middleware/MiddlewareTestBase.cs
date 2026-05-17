using System.Text.Json;

namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

public abstract class MiddlewareTestBase
{
    protected static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    protected static HttpClient Client => MiddlewareTestSetup.Client;
}
