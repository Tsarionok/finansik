using Finansik.Domain.UseCases.GetGroupMembers;
using FluentAssertions;

namespace Finansik.Domain.Tests.UseCases.GetGroupMembers;

public class GetGroupMembersQueryValidatorShould
{
    private readonly GetGroupMembersQueryValidator _sut = new();

    [Fact]
    async Task ReturnSuccess_WhenQueryIsValid()
    {
        var query = new GetGroupMembersQuery(Guid.Parse("448DD624-EE5E-4E0D-B334-BC57DD2171D3"));

        var actual = await _sut.ValidateAsync(query, CancellationToken.None);

        actual.IsValid.Should().BeTrue();
    }

    [Fact]
    async Task ReturnFailed_WhenQueryIsInvalid()
    {
        var query = new GetGroupMembersQuery(Guid.Empty);

        var actual = await _sut.ValidateAsync(query, CancellationToken.None);

        actual.IsValid.Should().BeFalse();
    }
}