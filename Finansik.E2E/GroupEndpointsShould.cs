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
        using var response = await httpClient.GetAsync("group");
        response.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();

        var content = JObject.FromObject(new
        {
            name = "E2E_testing",
            icon = "etoe"
        }).ToString();

        var request = new StringContent(content, Encoding.UTF8, "application/json");
        
        using var postResponse = await httpClient.PostAsync("group", request);
        postResponse.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();
        
        using var actual = await httpClient.GetAsync("group");
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