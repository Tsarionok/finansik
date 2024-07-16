using Finansik.Domain.Authentication;
using FluentAssertions;

namespace Finansik.Domain.Tests.Authentication;

public class PasswordManagerShould
{
    private readonly IPasswordManager _sut;
    private static readonly byte[] emptySalt = Enumerable.Repeat((byte)0, 100).ToArray();
    private static readonly byte[] emptyHash = Enumerable.Repeat((byte)0, 32).ToArray();
    
    public PasswordManagerShould()
    {
        _sut = new PasswordManager();
    }
    
    [Theory]
    [InlineData("password")]
    [InlineData("qwerty123")]
    public void GenerateMeaningfulSaltAndHash(string password)
    {
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        salt.Should().HaveCount(100).And.NotBeEquivalentTo(emptySalt);
        hash.Should().HaveCount(32).And.NotBeEquivalentTo(emptyHash);
    }

    [Fact]
    public void ReturnTrue_WhenPasswordMatch()
    {
        var password = "qwerty123";
        var (salt, hash) = _sut.GeneratePasswordParts(password);
        _sut.ComparePasswords(password, salt, hash).Should().BeTrue();
    }

    [Fact]
    public void ReturnFalse_WhenPasswordNotMatch()
    {
        var (salt, hash) = _sut.GeneratePasswordParts("asdf1234");
        _sut.ComparePasswords("alienPass", salt, hash).Should().BeFalse();
    }
}