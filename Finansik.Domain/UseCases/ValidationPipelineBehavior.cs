using FluentValidation;
using MediatR;

namespace Finansik.Domain.UseCases;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(IValidator<TRequest> validator) : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
        return await next.Invoke();
    }
}