using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Finansik.E2E;

public class MappingProfileShould : IClassFixture<FinansikApiApplicationFactory>
{
    private readonly FinansikApiApplicationFactory _factory;

    public MappingProfileShould(FinansikApiApplicationFactory factory)
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