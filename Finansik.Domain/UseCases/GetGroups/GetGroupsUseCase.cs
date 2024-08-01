using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;

namespace Finansik.Domain.UseCases.GetGroups;

internal class GetGroupsUseCase(
    IGetGroupsStorage storage,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager,
    DomainMetrics metrics) : IGetGroupsUseCase
{
    public async Task<IEnumerable<Group>> ExecuteAsync(CancellationToken cancellationToken)
    {
        // TODO: refactor metrics
        try
        {
            // intentionManager.ThrowIfForbidden(GroupIntention.Get);
        
            var result = await storage.GetGroupsByUserId(identityProvider.Current.UserId, cancellationToken);
            metrics.GroupsFetched(true);
            return result;
        }
        catch
        {
            metrics.GroupsFetched(false);
            throw;
        }
    }
}