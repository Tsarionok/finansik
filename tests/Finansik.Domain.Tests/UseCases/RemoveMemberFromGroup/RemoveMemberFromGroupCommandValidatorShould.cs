using Finansik.Domain.UseCases.RemoveMemberFromGroup;
using FluentAssertions;

namespace Finansik.Domain.Tests.UseCases.RemoveMemberFromGroup;

public class RemoveMemberFromGroupCommandValidatorShould
{
    private readonly RemoveMemberFromGroupCommandValidator _sut = new();
    
    [Fact]
    public async Task ReturnTrue_WhenCommandIsValid()
    {
        var command = new RemoveMemberFromGroupCommand(
            Guid.Parse("3CAFFDD2-5074-45B1-8E03-720D3FC1D2B0"),
            Guid.Parse("DB003F3E-2B28-4425-893E-244A541364F9"));

        var actual = await _sut.ValidateAsync(command, CancellationToken.None);
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new RemoveMemberFromGroupCommand(
            Guid.Parse("62613C8A-A79A-4079-A91B-4E223CD54FEB"),
            Guid.Parse("DC6BCB2A-B4D2-47C2-A303-97D73272280E"));

        yield return [validCommand with {UserId = Guid.Empty}];
        yield return [validCommand with {GroupId = Guid.Empty}];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandIsInvalid(RemoveMemberFromGroupCommand command)
    {
        var actual = await _sut.ValidateAsync(command, CancellationToken.None);
        actual.IsValid.Should().BeFalse();
    }
}