using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;

namespace Finansik.Domain.UseCases.CreateGroup;

public class GroupIntentionResolver : IIntentionResolver<GroupIntention>
{
    public bool IsAllowed(IIdentity subject, GroupIntention intention) => intention switch
    {
        GroupIntention.Create => subject.IsAuthenticated(),
        GroupIntention.Get => subject.IsAuthenticated(),
        _ => false
    };
}