using Finansik.Domain.Exceptions;
using Finansik.Domain.Exceptions.ErrorCodes;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Finansik.API.Middlewares.Extensions;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        IntentionManagerException exception) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status403Forbidden,
            "Authorization failed",
            exception.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        ValidationException exception)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in exception.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }
        
        return factory.CreateValidationProblemDetails(httpContext,
            modelStateDictionary,
            StatusCodes.Status400BadRequest,
            "Invalid request");
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        DomainException exception) =>
        factory.CreateProblemDetails(httpContext,
            statusCode: exception.ErrorCode switch
            {
                DomainErrorCodes.Gone => StatusCodes.Status410Gone,
                _ => StatusCodes.Status500InternalServerError
            },
            exception.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, HttpContext httpContext,
        Exception exception) =>
        factory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError,
            "Unhandled error! Please, contact us.");
}