using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetCategories;

internal class GetCategoriesByGroupIdUseCase(IGetCategoriesByGroupIdStorage storage) : IGetCategoriesByGroupIdUseCase
{
    public async Task<IEnumerable<Category>> ExecuteAsync(GetCategoriesByGroupIdCommand command, CancellationToken cancellationToken)
    {
        var groupExists = await storage.IsGroupExists(command.GroupId, cancellationToken);
        
        if (!groupExists)
        {
            throw new GroupNotFoundException(command.GroupId);
        }

        return await storage.GetCategoriesByGroupId(command.GroupId, cancellationToken);
    }
}