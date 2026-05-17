using System.Net;
using System.Net.Http.Json;
using AwesomeAssertions;
using AwesomeAssertions.Execution;
using Outbox.Api.Minimal.User.Models;

namespace Outbox.Api.Minimal.Tests.Integration.User;

public class GetUserByIdTests : IntegrationTestBase
{
    private const string UsersRoute = "/users";
    private const string ExpectedUserName = "Alice";

    [Test]
    public async Task Returns200_WithUserDetails_WhenUserExists()
    {
        var createResponse = await Client.PostAsJsonAsync(UsersRoute, new CreateUserRequest(ExpectedUserName));
        var created = await createResponse.Content.ReadFromJsonAsync<CreatedUserResponse>(JsonOptions);

        var response = await Client.GetAsync($"{UsersRoute}/{created!.Id}");
        var user = await response.Content.ReadFromJsonAsync<UserResponse>(JsonOptions);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user!.Id.Should().Be(created.Id);
            user.Name.Should().Be(ExpectedUserName);
        }
    }

    [Test]
    public async Task Returns404_WhenUserDoesNotExist()
    {
        var nonExistentId = Guid.NewGuid();

        var response = await Client.GetAsync($"{UsersRoute}/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
