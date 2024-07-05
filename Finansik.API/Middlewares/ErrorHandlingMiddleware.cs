using Finansik.Domain;
using Finansik.Domain.Exceptions;
using FluentValidation;

namespace Finansik.API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContent)
    {
        try
        {
            await next.Invoke(httpContent);
        }
        catch (Exception exception)
        {
            var statusCode = exception switch
            {
                IntentionManagerException => StatusCodes.Status403Forbidden,
                ValidationException => StatusCodes.Status400BadRequest,
                DomainException domainException => domainException.ErrorCode switch
                {
                    ErrorCodes.Gone => StatusCodes.Status410Gone,
                    ErrorCodes.Forbidden => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                },
                _ => StatusCodes.Status500InternalServerError
            };
            httpContent.Response.StatusCode = statusCode;
        }
    }
}