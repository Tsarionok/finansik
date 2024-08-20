using Finansik.API.Authentication;
using Finansik.Domain.Authentication;

namespace Finansik.API.Middlewares;

public sealed class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuthenticationService authenticationService,
        IIdentityProvider identityProvider,
        IAuthTokenStorage authTokenStorage)
    {
        var identity = authTokenStorage.TryExtract(httpContext, out var authToken)
            ? await authenticationService.Authenticate(authToken, httpContext.RequestAborted)
            : User.Guest;
        
        identityProvider.Current = identity;

        await next(httpContext);
    }
}