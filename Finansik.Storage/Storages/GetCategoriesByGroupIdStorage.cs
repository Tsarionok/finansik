using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetCategories;

namespace Finansik.Storage.Storages;

internal class GetCategoriesByGroupIdStorage : IGetCategoriesByGroupIdStorage
{
    public Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Category>> GetCategoriesByGroupId(Guid groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}