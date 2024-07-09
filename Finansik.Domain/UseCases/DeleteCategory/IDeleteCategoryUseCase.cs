using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.DeleteCategory;

public interface IDeleteCategoryUseCase : IUseCase<DeleteCategoryCommand, Category>;