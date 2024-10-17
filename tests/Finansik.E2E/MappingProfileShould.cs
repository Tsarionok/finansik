using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Finansik.E2E;

public class MappingProfileShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public MappingProfileShould(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public void BeValid()
    {
        _factory.Services.GetRequiredService<IMapper>().ConfigurationProvider
            .Invoking(p => p.AssertConfigurationIsValid())
            .Should()
            .NotThrow();
    }
}