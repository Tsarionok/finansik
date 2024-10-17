using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.RenameCategory;

public interface IRenameCategoryStorage
{
    Task<bool> IsCategoryExists(Guid categoryId, CancellationToken cancellationToken);
    
    Task<Category> RenameCategory(Guid categoryId, string nextName, CancellationToken cancellationToken);
}