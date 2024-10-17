using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetCategories;

public interface IGetCategoriesByGroupIdStorage
{
    Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken);

    Task<IEnumerable<Category>> GetCategoriesByGroupId(Guid groupId, CancellationToken cancellationToken);
}