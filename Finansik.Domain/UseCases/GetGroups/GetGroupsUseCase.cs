using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroups;

internal class GetGroupsUseCase(
    IGetGroupsStorage storage,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IRequestHandler<GetGroupsQuery, IEnumerable<Group>>
{
    public async Task<IEnumerable<Group>> Handle(GetGroupsQuery query, CancellationToken cancellationToken)
    {
        // intentionManager.ThrowIfForbidden(GroupIntention.Get);
        return await storage.GetGroupsByUserId(identityProvider.Current.UserId, cancellationToken);
    }
}