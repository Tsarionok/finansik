namespace Finansik.Domain.UseCases;

public interface IUseCase;

[Experimental]
public interface IUseCase<in TCommand, TResult> : IUseCase
    where TCommand : ICommand 
    where TResult : IResult
{
    Task<TResult> Execute(TCommand command, CancellationToken cancellationToken);
}

public interface IResult;
public interface ICommand;