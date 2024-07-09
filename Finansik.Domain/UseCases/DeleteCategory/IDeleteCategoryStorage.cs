using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.DeleteCategory;

public interface IDeleteCategoryStorage
{
    Task<Category> DeleteCategory(Guid categoryId, CancellationToken cancellationToken);
    
    Task<bool> IsCategoryExists(Guid categoryId, CancellationToken cancellationToken);
    
}