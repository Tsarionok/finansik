using Finansik.API.Authentication;
using Finansik.Domain.Authentication;

namespace Finansik.API.Middlewares;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuthenticationService authenticationService,
        IIdentityProvider identityProvider,
        IAuthTokenStorage authTokenStorage,
        CancellationToken cancellationToken)
    {
        var identity = authTokenStorage.TryExtract(httpContext, out var authToken)
            ? await authenticationService.Authenticate(authToken, cancellationToken)
            : User.Guest;
        
        identityProvider.Current = identity;

        await next(httpContext);
    }
}