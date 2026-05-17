using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Outbox.Api.Minimal.User.Models;

namespace Outbox.Api.Minimal.Tests.Integration.Outbox;

public class GetOutboxMessagesTests : IntegrationTestBase
{
    private const string UsersRoute = "/users";
    private const string OutboxRoute = "/outbox";
    private const string ExpectedUserName = "Alice";

    [Test]
    public async Task Returns200_WithMessages_AfterUserIsCreated()
    {
        await Client.PostAsJsonAsync(UsersRoute, new CreateUserRequest(ExpectedUserName));

        var response = await Client.GetAsync(OutboxRoute);
        var body = await response.Content.ReadFromJsonAsync<OutboxMessagesResponse>(JsonOptions);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            body!.Messages.Should().NotBeEmpty();
        }
    }
}