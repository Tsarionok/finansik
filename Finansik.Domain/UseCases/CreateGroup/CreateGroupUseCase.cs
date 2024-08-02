using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.CreateGroup;

internal class CreateGroupUseCase(
    ICreateGroupStorage createGroupStorage,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider) : IRequestHandler<CreateGroupCommand, Group>
{
    public async Task<Group> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(GroupIntention.Create);
        
        return await createGroupStorage.CreateGroup(command.Name, identityProvider.Current.UserId, command.Icon, cancellationToken);
    }
}