using Finansik.Domain.Attributes;

namespace Finansik.Domain.UseCases;

public interface IUseCase;

public interface IUseCase<TResult> : IUseCase
    where TResult : class
{
    Task<TResult> ExecuteAsync(CancellationToken cancellationToken);
}

[Experimental]
public interface IUseCase<in TInput, TResult> : IUseCase
    where TInput : IInput
    where TResult : class
{
    Task<TResult> ExecuteAsync(TInput command, CancellationToken cancellationToken);
}

public interface IInput;
public interface ICommand : IInput;
public interface IQuery : IInput;