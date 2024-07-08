using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.DeleteCategory;

public interface IDeleteCategoryUseCase
{ 
    Task<Category> DeleteCategory(Guid categoryId, CancellationToken cancellationToken);
}