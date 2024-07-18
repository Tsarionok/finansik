using Finansik.Domain.UseCases.SignOn;
using FluentAssertions;
using JetBrains.Annotations;

namespace Finansik.Domain.Tests.UseCases.SignOn;

[TestSubject(typeof(SignOnCommandValidator))]
public class SignOnCommandValidatorShould
{
    private readonly SignOnCommandValidator _sut;
    
    public SignOnCommandValidatorShould()
    {
        _sut = new SignOnCommandValidator();
    }

    [Fact]
    public async Task ReturnSuccess_WhenCommandIsValid()
    {
        var command = new SignOnCommand("kava", "superPass1234");

        var actual = await _sut.ValidateAsync(command);
        actual.IsValid.Should().BeTrue();
    }
    
    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new SignOnCommand("Validich", "PassMega000");

        yield return [validCommand with {Login = string.Empty}];
        yield return [validCommand with {Password = string.Empty}];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFailure_WhenCommandIsInvalid(SignOnCommand command)
    {
        var actual = await _sut.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
}