using IronPython.Hosting;
using MediatR;

namespace Finansik.Domain.UseCases.ExecuteCode;

public class ExecuteCodeUseCase : IRequestHandler<ExecuteCodeQuery, ExecuteCodeQueryResult>
{
    public Task<ExecuteCodeQueryResult> Handle(ExecuteCodeQuery query, CancellationToken cancellationToken)
    {
        var engine = Python.CreateEngine();
        var scope = engine.CreateScope();
 
        engine.Execute(query.Code, scope);
        var startMethod = scope.GetVariable(query.StartMethod);
        
        var result = startMethod(query.Argument);

        return result;
    }
}