using Finansik.Domain.UseCases.GetGroupById;
using FluentAssertions;

namespace Finansik.Domain.Tests.UseCases.GetGroup;

public class GetGroupByIdQueryValidatorShould
{
    private readonly GetGroupByIdQueryValidator _sut = new();

    [Fact]
    public async Task ReturnSuccess_WhenQueryIsValid()
    {
        var validQuery = new GetGroupByIdQuery(Guid.Parse("9ACBB88B-9C3E-4C27-82F3-7D1A318D2942"));

        var actual = await _sut.ValidateAsync(validQuery);

        actual.IsValid.Should().BeTrue();
    }
    
    [Fact]
    public async Task ReturnFailed_WhenQueryIsInvalid()
    {
        var invalidQuery = new GetGroupByIdQuery(Guid.Empty);

        var actual = await _sut.ValidateAsync(invalidQuery);

        actual.IsValid.Should().BeFalse();
    }
}