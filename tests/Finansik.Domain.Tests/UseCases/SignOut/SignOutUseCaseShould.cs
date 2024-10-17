using Finansik.Domain.Authentication;
using Finansik.Domain.UseCases.SignOut;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.SignOut;

[TestSubject(typeof(SignOutUseCase))]
public class SignOutUseCaseShould
{
    private readonly SignOutUseCase _sut;
    private readonly ISetup<ISignOutStorage,Task> _removeSessionSetup;
    private readonly Mock<ISignOutStorage> _storage;
    private readonly ISetup<IIdentityProvider,IIdentity> _currentIdentitySetup;

    public SignOutUseCaseShould()
    {
        _storage = new Mock<ISignOutStorage>();
        _removeSessionSetup = _storage.Setup(s => s.RemoveSession(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identityProvider = new Mock<IIdentityProvider>();
        _currentIdentitySetup = identityProvider.Setup(p => p.Current);
        
        _sut = new SignOutUseCase(
            _storage.Object, 
            identityProvider.Object);
    }

    [Fact]
    public async Task ResetCurrentIdentitySession()
    {
        var sessionId = Guid.Parse("96B1C7C4-7AFD-4518-932E-BEA4861B7D00");
        _removeSessionSetup.Returns(Task.CompletedTask);
        _currentIdentitySetup.Returns(new User(Guid.Parse("D0C40570-5DD9-4818-8262-25E80C667951"), sessionId));
        
        await _sut.Handle(new SignOutCommand(), CancellationToken.None);
        
        _storage.Verify(s => s.RemoveSession(sessionId, It.IsAny<CancellationToken>()), Times.Once);
    }
}