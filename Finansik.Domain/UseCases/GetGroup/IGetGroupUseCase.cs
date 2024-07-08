namespace Finansik.Domain.UseCases.GetGroup;

internal class GetGroupUseCase : IUseCase<GetGroupCommand, GetGroupResult>
{
    public async Task<GetGroupResult> Execute(GetGroupCommand command, CancellationToken cancellationToken)
    {
        return new GetGroupResult();
    }
}

public class GetGroupCommand : ICommand
{
    
}

public class GetGroupResult : IResult
{
    
}