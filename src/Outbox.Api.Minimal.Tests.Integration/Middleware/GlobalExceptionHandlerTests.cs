using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Microsoft.AspNetCore.Http;

namespace Outbox.Api.Minimal.Tests.Integration.Middleware;

public class GlobalExceptionHandlerTests : MiddlewareTestBase
{
    private const string UsersRoute = "/users";
    private const string ExpectedUserName = "Alice";
    private const string ProblemDetailsContentType = "application/problem+json";
    private const string SimulatedExceptionMessage = "Simulated unhandled exception";
    private const string ExpectedTitle = "An error occurred while processing your request";

    [Test]
    public async Task Returns500_WithProblemDetails_WhenUnhandledExceptionOccurs()
    {
        var request = new { name = ExpectedUserName };

        var response = await Client.PostAsJsonAsync(UsersRoute, request);
        var body = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>(JsonOptions);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            response.Content.Headers.ContentType!.MediaType.Should().Be(ProblemDetailsContentType);
            body!.Title.Should().Be(ExpectedTitle);
            body.Status.Should().Be(StatusCodes.Status500InternalServerError);
            body.Detail.Should().Be(SimulatedExceptionMessage);
        }
    }
}
