using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroup;

internal class GetGroupUseCase : IGetGroupUseCase
{
    public async Task<Group> ExecuteAsync(GetGroupQuery query, CancellationToken cancellationToken)
    {
        return new Group
        {
            Name = ""
        };
    }
}