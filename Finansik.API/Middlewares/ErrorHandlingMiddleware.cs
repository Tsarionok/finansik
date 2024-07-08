using Finansik.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Finansik.API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    // ReSharper disable once UnusedMember.Global
    public async Task InvokeAsync(
        HttpContext httpContext, 
        ILogger<ErrorHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            logger.LogDebug(
                "Error handling started with path {RequestPath}",
                httpContext.Request.Path.Value);
            
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            ProblemDetails problemDetails;
            switch (exception)
            {
                case IntentionManagerException intentionManagerException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, intentionManagerException);
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    break;
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                    logger.LogError(domainException, "Domain exception occured");
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, exception);
                    logger.LogError(exception, "Unhandled exception occured");
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}