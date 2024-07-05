using Finansik.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Finansik.API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    // ReSharper disable once UnusedMember.Global
    public async Task InvokeAsync(
        HttpContext httpContext, 
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(httpContext);
        }
        catch (Exception exception)
        {
            var problemDetails = exception switch
            {
                IntentionManagerException intentionManagerException => 
                    problemDetailsFactory.CreateFrom(httpContext, intentionManagerException),
                ValidationException validationException => 
                    problemDetailsFactory.CreateFrom(httpContext, validationException),
                DomainException domainException => 
                    problemDetailsFactory.CreateFrom(httpContext, domainException),
                _ => problemDetailsFactory.CreateFrom(httpContext, exception)
            };
            
            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}