using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetCategories;

public class GetCategoriesByGroupIdUseCase(IGetCategoriesByGroupIdStorage storage) : IGetCategoriesByGroupIdUseCase
{
    public async Task<IEnumerable<Category>> Execute(Guid groupId, CancellationToken cancellationToken)
    {
        var groupExists = await storage.IsGroupExists(groupId, cancellationToken);
        
        if (!groupExists)
        {
            throw new GroupNotFoundException(groupId);
        }

        return await storage.GetCategoriesByGroupId(groupId, cancellationToken);
    }
}