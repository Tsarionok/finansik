using System.Net.Http.Json;
using System.Text;
using Finansik.Storage;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Newtonsoft.Json.Linq;

namespace Finansik.E2E;

public class GroupEndpointsShould : IClassFixture<FinansikApiApplicationFactory>
{
    private readonly FinansikApiApplicationFactory _factory;
    private readonly ISetup<IGuidFactory,Guid> _guidCreateSetup;

    // ReSharper disable once ConvertToPrimaryConstructor
    public GroupEndpointsShould(FinansikApiApplicationFactory factory)
    {
        _factory = factory;

        var guidFactory = new Mock<IGuidFactory>();
        _guidCreateSetup = guidFactory.Setup(f => f.Create());
    }
    
    [Fact]
    public async Task ReturnListOfCreatedGroups_WhenPostGroup()
    {
        var groupId = Guid.Parse("3475DE72-BB92-4106-A3BC-13D8A5B2449E");
        _guidCreateSetup.Returns(groupId);
        
        using var httpClient = _factory.CreateClient();

        // Sign-on
        using var signOnResponse = await httpClient.PostAsync("account/signon", JsonContent.Create(new
        {
            login = "tester",
            password = "qwerty"
        }));
        signOnResponse.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        
        // Sign-in
        using var signInResponse = await httpClient.PostAsync("account/signin", JsonContent.Create(new
        {
            login = "tester",
            password = "qwerty"
        }));
        signInResponse.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        
        // Get groups
        using var response = await httpClient.GetAsync("groups");
        response.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        
        // Post group
        var content = JObject.FromObject(new
        {
            name = "E2E_testing",
            icon = "etoe"
        }).ToString();
        var request = new StringContent(content, Encoding.UTF8, "application/json");
        using var postResponse = await httpClient.PostAsync("groups", request);
        postResponse.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        
        // Get groups
        using var actual = await httpClient.GetAsync("groups");
        actual.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        var result = await actual.Content.ReadAsStringAsync();
        JArray.Parse(result).First
            .Should()
            .BeEquivalentTo(JToken.FromObject(new
            {
                id = groupId,
                name = "E2E_testing",
                icon = "etoe"
            }));
    }
}