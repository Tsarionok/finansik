using System.Security.Cryptography;
using System.Text;
using Finansik.Domain.Authentication;
using Finansik.Domain.Authentication.Cryptography;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.SignIn;
using FluentAssertions;
using FluentValidation;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.SignIn;

[TestSubject(typeof(SignInUseCase))]
public class SignInUseCaseShould
{
    private readonly SignInUseCase _sut;
    private readonly ISetup<ISignInStorage,Task<RecognisedUser?>> _findUserSetup;
    private readonly ISetup<IPasswordManager,bool> _comparePasswordsSetup;
    private readonly ISetup<ISymmetricEncryptor,Task<string>> _encryptSetup;

    public SignInUseCaseShould()
    {
        var validator = new Mock<IValidator<SignInCommand>>();
        var storage = new Mock<ISignInStorage>();
        var passwordManager = new Mock<IPasswordManager>();
        var encryptor = new Mock<ISymmetricEncryptor>();
        var authenticationConfig = new Mock<IOptions<AuthenticationConfiguration>>();

        _findUserSetup = storage.Setup(s => 
            s.FindUser(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        _comparePasswordsSetup = passwordManager.Setup(m => 
            m.ComparePasswords(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        _encryptSetup = encryptor.Setup(e => 
            e.Encrypt(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        authenticationConfig.Setup(c => c.Value).Returns(new AuthenticationConfiguration
        {
            Base64Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32))
        });
        
        _sut = new SignInUseCase(
            validator.Object, storage.Object, passwordManager.Object, encryptor.Object, authenticationConfig.Object);
    }

    [Fact]
    public async Task ThrowUserNotRecognizedException_WhenUserNotRecognized()
    {
        _findUserSetup.ReturnsAsync((RecognisedUser?) null);

        var actual = _sut.Invoking(
            async sut => await sut.Execute(new SignInCommand("superMegaUser", "12345"), CancellationToken.None));

        await actual.Should().ThrowAsync<UserNotRecognizedException>();
    }

    [Fact]
    public async Task ThrowPasswordNotMatchedException_WhenPasswordNotMatched()
    {
        _findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = Guid.Parse("F994D3FF-47CA-41A8-B798-587356833987"),
            Salt = RandomNumberGenerator.GetBytes(128),
            PasswordHash = RandomNumberGenerator.GetBytes(32)
        });
        _comparePasswordsSetup.Returns(false);

        var actual = _sut.Invoking(async sut => 
            await sut.Execute(new SignInCommand("registered", "brutforcedPass"), CancellationToken.None));

        await actual.Should().ThrowAsync<PasswordNotMatchedException>();
    }

    [Fact]
    public async Task ReturnUserIdWithAuthToken_WhenPasswordsMathed()
    {
        var userId = Guid.Parse("0549A929-6080-4E09-9284-FBB15D8285C3");
        var authenticationToken = "someSecureHashedToken";
        
        _findUserSetup.ReturnsAsync(new RecognisedUser
        {
            UserId = userId,
            Salt = RandomNumberGenerator.GetBytes(128),
            PasswordHash = RandomNumberGenerator.GetBytes(32)
        });
        _comparePasswordsSetup.Returns(true);
        _encryptSetup.ReturnsAsync(authenticationToken);
        
        var (actualIdentity, actualToken)  = await _sut.Execute(new SignInCommand("admin", "admin"), CancellationToken.None);

        actualIdentity.UserId.Should().Be(userId);
        actualToken.Should().Be(authenticationToken);
    }
}