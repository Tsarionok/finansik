using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateGroup;

namespace Finansik.Domain.UseCases.GetGroups;

public class GetGroupsUseCase(
    IGetGroupsStorage storage,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IGetGroupsUseCase
{
    public async Task<IEnumerable<Group>> Execute(CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(GroupIntention.Get);
        
        return await storage.GetGroupsByUserId(identityProvider.Current.UserId, cancellationToken);
    }
}