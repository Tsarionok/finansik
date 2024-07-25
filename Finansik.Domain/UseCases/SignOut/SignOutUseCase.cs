using Finansik.Domain.Authentication;

namespace Finansik.Domain.UseCases.SignOut;

public class SignOutUseCase(
    ISignOutStorage storage, 
    IIdentityProvider identityProvider) : ISignOutUseCase
{
    public async Task Execute(SignOutCommand command, CancellationToken cancellationToken) =>
        await storage.RemoveSession(identityProvider.Current.SessionId, cancellationToken);
}