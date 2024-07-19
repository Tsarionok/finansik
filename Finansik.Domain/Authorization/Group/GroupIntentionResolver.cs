using Finansik.Domain.Authentication;
using Finansik.Domain.UseCases.CreateGroup;

namespace Finansik.Domain.Authorization.Group;

internal class GroupIntentionResolver : IIntentionResolver<GroupIntention>
{
    public bool IsAllowed(IIdentity subject, GroupIntention intention) => intention switch
    {
        GroupIntention.Create => subject.IsAuthenticated(),
        GroupIntention.Get => subject.IsAuthenticated(),
        _ => false
    };
}