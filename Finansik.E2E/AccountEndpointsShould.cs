using System.Net.Http.Json;
using Finansik.Domain.Authentication;
using FluentAssertions;

namespace Finansik.E2E;

public class AccountEndpointsShould(FinansikApiApplicationFactory factory) : 
    IClassFixture<FinansikApiApplicationFactory>
{
    [Fact]
    public async Task SignInAfterSignOn()
    {
        // TODO: refactor to Given-When-Then style (BDD, Gherkin)
        using var httpClient = factory.CreateClient();

        using var signOnResponse = await httpClient.PostAsync("account/signon", JsonContent.Create(new
        {
            login = "Test",
            password = "qwerty"
        }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        var createdUser = await signOnResponse.Content.ReadFromJsonAsync<User>();

        using var signInResponse = await httpClient.PostAsync("account/signin", JsonContent.Create(new
        {
            login = "Test",
            password = "qwerty"
        }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();

        var signedInUser = await signInResponse.Content.ReadFromJsonAsync<User>();
        signedInUser!.UserId.Should().Be(createdUser!.UserId);

        using var createGroupResponse = await httpClient.PostAsync("group", JsonContent.Create(new
        {
            name = "Peronal",
            icon = "private.png"
        }));

        createGroupResponse.IsSuccessStatusCode.Should().BeTrue();
    }
}