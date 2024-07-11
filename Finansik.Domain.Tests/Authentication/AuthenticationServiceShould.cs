using Finansik.Domain.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.Authentication;

public class AuthenticationServiceShould
{
    private readonly AuthenticationService _sut;
    private readonly ISetup<IAuthenticationStorage,Task<RecognisedUser?>> _findUserSetup;

    public AuthenticationServiceShould()
    {
        var storage = new Mock<IAuthenticationStorage>();
        _findUserSetup = storage.Setup(s => s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        var securityManager = new Mock<ISecurityManager>();
        securityManager
            .Setup(s => s.ComparePasswords(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(true);
        
        var options = new Mock<IOptions<AuthenticationConfiguration>>();
        options
            .Setup(o => o.Value)
            .Returns(new AuthenticationConfiguration
            {
                Key = "wQxKcFiN9Y6QZMZ9n2SESNTPAndjkTfT",
                Iv = "dtEzMsz2ogg="
            });

        _sut = new AuthenticationService(storage.Object, securityManager.Object, options.Object);
    }
    
    [Fact]
    public async Task ReturnSuccess_WhenUserFound()
    {
        _findUserSetup.ReturnsAsync(new RecognisedUser
        {
            PasswordHash =
                "HsyBXi2q9wcFpjFBgNOgsBYvLQ6LmWXLOASJ5dEhiyn60bhBdzjJ4evUIB0Wb8rl8qQYWz7f9dK1LOUbzJRuTlcwl65srrT2eyKht5Mk8HG+tABr9bjW4e9pgDng/T2YZ4yBlg==",
            Salt = "voRw+86vXesnx4z0Y5TigXVuJzc=",
            UserId = Guid.Parse("970D2A5A-098F-4FD6-AFEF-7B6D9FD4B586")
        });

        var (success, authToken) = await _sut.SignIn(new SignInCredentials("User", "Password"), CancellationToken.None);
        success.Should().BeTrue();
        authToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AuthenticateUser_AfterTheySignIn()
    {
        var userId = Guid.Parse("764CB42B-F2D0-4654-9F36-1E00547E6AE8");
        _findUserSetup.ReturnsAsync(new RecognisedUser { UserId = userId });
        var (_, authToken) = await _sut.SignIn(new SignInCredentials("User", "Password"), CancellationToken.None);

        var identity = await _sut.Authenticate(authToken, CancellationToken.None);
        identity.UserId.Should().Be(userId);
    }
}