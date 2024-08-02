using Finansik.Domain.Authentication;
using MediatR;

namespace Finansik.Domain.UseCases.SignOut;

public class SignOutUseCase(
    ISignOutStorage storage, 
    IIdentityProvider identityProvider) : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand command, CancellationToken cancellationToken) =>
        await storage.RemoveSession(identityProvider.Current.SessionId, cancellationToken);
}