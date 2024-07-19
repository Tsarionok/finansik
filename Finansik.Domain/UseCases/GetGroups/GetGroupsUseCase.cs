using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroups;

internal class GetGroupsUseCase(
    IGetGroupsStorage storage,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IGetGroupsUseCase
{
    public async Task<IEnumerable<Group>> ExecuteAsync(CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(GroupIntention.Get);
        
        return await storage.GetGroupsByUserId(identityProvider.Current.UserId, cancellationToken);
    }
}