using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;

namespace Finansik.Domain.UseCases.CreateCategory;

public class CategoryIntentionResolver : IIntentionResolver<CategoryIntention>
{
    public bool IsAllowed(IIdentity subject, CategoryIntention intention) => intention switch
    {
        CategoryIntention.Create => subject.IsAuthenticated(),
        CategoryIntention.Rename => subject.IsAuthenticated(),
        _ => false
    };
}