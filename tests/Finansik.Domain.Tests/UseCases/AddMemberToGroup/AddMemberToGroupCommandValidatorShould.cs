using Finansik.Domain.UseCases.AddMemberToGroup;
using FluentAssertions;

namespace Finansik.Domain.Tests.UseCases.AddMemberToGroup;

public class AddMemberToGroupCommandValidatorShould
{
    private readonly AddMemberToGroupCommandValidator _sut = new();

    [Fact]
    public async Task ReturnSuccess_WhenCommandIsValid()
    {
        var command = new AddMemberToGroupCommand(
            Guid.Parse("39ED5CBF-2326-449C-A70F-B7945F5C036A"),
            Guid.Parse("2E3E354C-F61A-4B9F-8CD7-7571E24AD8A2"));

        var actual = await _sut.ValidateAsync(command, CancellationToken.None);

        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new AddMemberToGroupCommand(
            Guid.Parse("C72D2BD5-BFBA-474E-8485-6A8CD85BD195"),
            Guid.Parse("2F93DD06-0E9A-483C-A4D3-275EC60A8BFA"));

        yield return [ validCommand with {UserId = Guid.Empty} ];
        yield return [validCommand with {GroupId = Guid.Empty} ];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFailed_WhenCommandIsInvalid(AddMemberToGroupCommand command)
    {
        var actual = await _sut.ValidateAsync(command);

        actual.IsValid.Should().BeFalse();
    }
}