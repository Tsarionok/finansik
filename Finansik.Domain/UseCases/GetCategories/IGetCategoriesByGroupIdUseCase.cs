using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetCategories;

public interface IGetCategoriesByGroupIdUseCase
{
    Task<IEnumerable<Category>> Execute(Guid groupId, CancellationToken cancellationToken);
}