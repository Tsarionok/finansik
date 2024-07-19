using Finansik.Domain.UseCases.SignIn;
using FluentAssertions;
using JetBrains.Annotations;

namespace Finansik.Domain.Tests.UseCases.SignIn;

[TestSubject(typeof(SignInCommandValidator))]
public class SignInCommandValidatorShould
{
    private readonly SignInCommandValidator _sut;

    public SignInCommandValidatorShould()
    {
        _sut = new SignInCommandValidator();
    }

    [Fact]
    public async Task ReturnSuccess_WhenCommandIsValid()
    {
        var validCommand = new SignInCommand("admin", "securePassMega!222");

        var actual = await _sut.ValidateAsync(validCommand);

        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new SignInCommand("programator", "qswdeswsde!QWSll123");

        yield return [validCommand with {Login = string.Empty}];
        yield return [validCommand with {Password = string.Empty}];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFailed_WhenCommandIsInvalid(SignInCommand command)
    {
        var actual = await _sut.ValidateAsync(command);
        
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should().Contain(e => e.ErrorCode == ValidationErrorCode.Empty);
    }
}