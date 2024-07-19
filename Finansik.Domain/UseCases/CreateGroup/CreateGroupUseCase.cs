using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateGroup;

internal class CreateGroupUseCase(
    ICreateGroupStorage createGroupStorage,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider) : ICreateGroupUseCase
{
    public async Task<Group> ExecuteAsync(CreateGroupCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(GroupIntention.Create);
        
        return await createGroupStorage.CreateGroup(command.Name, identityProvider.Current.UserId, command.Icon, cancellationToken);
    }
}