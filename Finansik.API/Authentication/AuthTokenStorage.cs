namespace Finansik.API.Authentication;

internal class AuthTokenStorage : IAuthTokenStorage
{
    private const string Key = "Finansik-Auth-Token";
    
    public bool TryExtract(HttpContext httpContext, out string token)
    {
        if (httpContext.Request.Cookies.TryGetValue(Key, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            token = value;
            return true;
        }

        token = string.Empty;
        return false;
    }

    public void Store(HttpContext httpContext, string token) => 
        httpContext.Response.Cookies.Append(Key, token);
}