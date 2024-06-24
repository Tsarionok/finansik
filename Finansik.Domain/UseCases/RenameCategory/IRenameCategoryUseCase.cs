using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.RenameCategory;

public interface IRenameCategoryUseCase
{
    Task<Category> Execute(Guid categoryId, string nextName, CancellationToken cancellationToken);
}