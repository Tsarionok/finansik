using Finansik.Domain.Authentication;
using Finansik.Domain.Exceptions;

namespace Finansik.Domain.Authorization;

internal class IntentionManager(IEnumerable<IIntentionResolver> resolvers, IIdentityProvider identityProvider) : IIntentionManager
{
    public bool IsAllowed<TIntention>(TIntention intention) where TIntention : struct
    {
        var matchingResolver = resolvers.OfType<IIntentionResolver<TIntention>>().FirstOrDefault();
        return matchingResolver?.IsAllowed(identityProvider.Current, intention) ?? false;
    }

    public bool IsAllowed<TIntention, TObject>(TIntention intention, TObject target) where TIntention : struct
    {
        throw new NotImplementedException();
    }
}

internal static class IntentionManagerExtensions
{
    public static void ThrowIfForbidden<TIntention>(this IIntentionManager intentionManager, TIntention intention) where TIntention : struct 
    {
        if (!intentionManager.IsAllowed(intention))
        {
            throw new IntentionManagerException();
        }
    }
}