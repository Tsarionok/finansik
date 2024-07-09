using FluentAssertions;

namespace Finansik.E2E;

public class GroupEndpointsShould : IClassFixture<FinansikApiApplicationFactory>
{
    private readonly FinansikApiApplicationFactory _factory;

    // ReSharper disable once ConvertToPrimaryConstructor
    public GroupEndpointsShould(FinansikApiApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task ReturnListOfGroups()
    {
        using var httpClient = _factory.CreateClient();
        using var response = await httpClient.GetAsync("group");
        response.Invoking(res => res.EnsureSuccessStatusCode()).Should().NotThrow<HttpRequestException>();

        var result = await response.Content.ReadAsStringAsync();
        result.Should().BeEquivalentTo("[]");
    }
}