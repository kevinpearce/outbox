using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Outbox.Api.Minimal.User.Models;

namespace Outbox.Api.Minimal.Tests.Integration.User;

public class CreateUserTests : IntegrationTestBase
{
    private const string UsersRoute = "/users";
    private const string ExpectedUserName = "Alice";
    private const string NameExceedingMaxLength = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"; // 51 chars

    [Test]
    public async Task Returns201_WithLocationAndBody_WhenNameIsValid()
    {
        var request = new CreateUserRequest(ExpectedUserName);

        var response = await Client.PostAsJsonAsync(UsersRoute, request);
        var body = await response.Content.ReadFromJsonAsync<CreatedUserResponse>(JsonOptions);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location!.ToString().Should().Contain(body!.Id.ToString());
            body.Id.Should().NotBeEmpty();
        }
    }

    [TestCase("")]
    [TestCase(NameExceedingMaxLength)]
    public async Task Returns400_WhenNameIsInvalid(string name)
    {
        var request = new CreateUserRequest(name);

        var response = await Client.PostAsJsonAsync(UsersRoute, request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
