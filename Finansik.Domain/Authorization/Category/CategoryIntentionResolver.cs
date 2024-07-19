using Finansik.Domain.Authentication;

namespace Finansik.Domain.Authorization.Category;

internal class CategoryIntentionResolver : IIntentionResolver<CategoryIntention>
{
    public bool IsAllowed(IIdentity subject, CategoryIntention intention) => intention switch
    {
        CategoryIntention.Create => subject.IsAuthenticated(),
        CategoryIntention.Rename => subject.IsAuthenticated(),
        CategoryIntention.Delete => subject.IsAuthenticated(),
        _ => false
    };
}