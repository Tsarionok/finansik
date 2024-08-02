using Finansik.Domain.Authentication;
using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.SignOn;
using FluentAssertions;
using FluentValidation;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.SignOn;

[TestSubject(typeof(SignOnUseCase))]
public class SignOnUseCaseShould
{
    private readonly SignOnUseCase _sut;
    private readonly ISetup<ISignOnStorage,Task<bool>> _userExistsSetup;
    private readonly ISetup<IPasswordManager,bool> _comparePasswordsSetup;
    private readonly ISetup<ISignOnStorage,Task<Guid>> _createUserSetup;
    private readonly ISetup<IPasswordManager,(byte[] salt, byte[] hash)> _generatePasswordPartsSetup;
    private readonly ISetup<IPasswordManager> _passwordNotMatchedSetup;

    public SignOnUseCaseShould()
    {
        var validator = new Mock<IValidator<SignOnCommand>>();
        var storage = new Mock<ISignOnStorage>();
        _userExistsSetup = storage.Setup(s => 
            s.IsLoginAlreadyUsed(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        _createUserSetup = storage.Setup(s =>
            s.CreateUser(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>(), It.IsAny<CancellationToken>()));

        var passwordManager = new Mock<IPasswordManager>();
        _passwordNotMatchedSetup = passwordManager.Setup(m =>
            m.ThrowIfPasswordNotMatched(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()));
        _generatePasswordPartsSetup = passwordManager.Setup(m => m.GeneratePasswordParts(It.IsAny<string>()));
        
        _sut = new SignOnUseCase(validator.Object, storage.Object, passwordManager.Object);
    }

    [Fact]
    public async Task ThrowLoginAlreadyUsedException_WhenLoginAlreadyExists()
    {
        _userExistsSetup.ReturnsAsync(true);
        await _sut.Invoking(sut => sut.Handle(new SignOnCommand("admin", "admin"), CancellationToken.None))
            .Should().ThrowAsync<LoginAlreadyUsedException>();
    }

    [Fact]
    public async Task ReturnNotEmptyUserId_WhenUserCreatedSuccessfully()
    {
        var userId = Guid.Parse("DBF060D6-EF1D-4483-927B-BEE9301B1CD7");
        _userExistsSetup.ReturnsAsync(false);
        _createUserSetup.ReturnsAsync(userId);

        var actual = await _sut.Handle(new SignOnCommand("operator", "qwerty1234"), CancellationToken.None);
        actual.UserId.Should().Be(userId);
    }
}