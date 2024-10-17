using Finansik.Domain.Authentication;
using Finansik.Domain.Exceptions;
using FluentAssertions;

namespace Finansik.Domain.Tests.Authentication;

public class PasswordManagerShould
{
    private readonly PasswordManager _sut = new();
    private static readonly byte[] EmptySalt = Enumerable.Repeat((byte)0, 100).ToArray();
    private static readonly byte[] EmptyHash = Enumerable.Repeat((byte)0, 32).ToArray();

    [Theory]
    [InlineData("password")]
    [InlineData("qwerty123")]
    public void GenerateMeaningfulSaltAndHash(string password)
    {
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        salt.Should().HaveCount(100).And.NotBeEquivalentTo(EmptySalt);
        hash.Should().HaveCount(32).And.NotBeEquivalentTo(EmptyHash);
    }

    [Fact]
    public void ReturnSuccess_WhenPasswordMatch()
    {
        var password = "qwerty123";
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        _sut.Invoking(sut => sut.ThrowIfPasswordNotMatched(password, salt, hash))
            .Should()
            .NotThrow();
    }

    [Fact]
    public void ThrowPasswordNotMatchedException_WhenPasswordNotMatch()
    {
        var (salt, hash) = _sut.GeneratePasswordParts("asdf1234");
        _sut.Invoking(sut => sut.ThrowIfPasswordNotMatched("alienPass", salt, hash))
            .Should()
            .Throw<PasswordNotMatchedException>();
    }
}