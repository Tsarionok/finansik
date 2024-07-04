using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateGroup;

public class CreateGroupUseCase(
    ICreateGroupStorage createGroupStorage,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider) : ICreateGroupUseCase
{
    public async Task<Group> Execute(string name, string icon, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(GroupIntention.Create);
        
        return await createGroupStorage.CreateGroup(name, identityProvider.Current.UserId, icon, cancellationToken);
    }
}