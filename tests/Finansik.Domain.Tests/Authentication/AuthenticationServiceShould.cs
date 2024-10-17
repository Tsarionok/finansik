using System.Security.Cryptography;
using Finansik.Domain.Authentication;
using Finansik.Domain.Authentication.Cryptography;
using Finansik.Domain.Models;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.Authentication;

[TestSubject(typeof(AuthenticationService))]
public class AuthenticationServiceShould
{
    private readonly AuthenticationService _sut;
    private readonly ISetup<ISymmetricDecryptor,Task<string>> _decryptSetup;
    private readonly ISetup<IAuthenticationStorage,Task<Session?>> _findSessionSetup;

    public AuthenticationServiceShould()
    {
        var decryptor = new Mock<ISymmetricDecryptor>();
        var storage = new Mock<IAuthenticationStorage>();

        _decryptSetup = decryptor.Setup(d => 
            d.Decrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));
        _findSessionSetup = storage.Setup(s => 
            s.FindSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        var options = new OptionsWrapper<AuthenticationConfiguration>(new AuthenticationConfiguration());
        options.Value.Base64Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        _sut = new AuthenticationService(
            decryptor.Object, storage.Object, NullLogger<AuthenticationService>.Instance, options);
    }

    [Fact]
    public async Task ReturnGuestUser_WhenTokenCannotBeDecrypted()
    {
        _decryptSetup.Throws<CryptographicException>();

        var actual = await _sut.Authenticate("invalid-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(User.Guest);
    }

    [Fact]
    public async Task ReturnGuestUser_WhenDecryptedTokenIsNotGuid()
    {
        _decryptSetup.ReturnsAsync("This string cannot be parsed as GUID");

        var actual = await _sut.Authenticate("bad-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(User.Guest);
    }

    [Fact]
    public async Task ReturnGuestUser_WhenSessionNotFound()
    {
        _decryptSetup.ReturnsAsync("3FEBD7E5-8C0C-4906-B9D9-0773A75BFD51");
        _findSessionSetup.ReturnsAsync(() => null);

        var actual = await _sut.Authenticate("valid-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(User.Guest);
    }

    [Fact]
    public async Task ReturnGuestUser_WhenSessionExpired()
    {
        _decryptSetup.ReturnsAsync("71A604F5-C8E4-4994-82E5-28023E484BED");
        _findSessionSetup.ReturnsAsync(new Session
        {
            UserId = Guid.Parse("756C9DA6-4F88-4789-AC05-BEC7CE84ACA1"),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(-2)
        });

        var actual = await _sut.Authenticate("expired-token", CancellationToken.None);

        actual.Should().BeEquivalentTo(User.Guest);
    }

    [Fact]
    public async Task ReturnRecognizedUser_WhenTokenIsMatched()
    {
        var userId = Guid.Parse("4AB1141E-3550-452B-B6A7-66B116D9359D");
        var sessionId = "9EFD13D1-2681-484C-8C29-B2165BF99AEC";
        _decryptSetup.ReturnsAsync(sessionId);
        _findSessionSetup.ReturnsAsync(new Session
        {

            UserId = userId,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(3)
        });

        var actual = await _sut.Authenticate("good-token", CancellationToken.None);

        actual.UserId.Should().Be(userId);
        actual.SessionId.Should().Be(Guid.Parse(sessionId));
    }
}