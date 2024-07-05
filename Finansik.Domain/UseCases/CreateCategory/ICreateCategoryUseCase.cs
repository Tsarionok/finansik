using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public interface ICreateCategoryUseCase
{
    Task<Category> Execute(CreateCategoryCommand command, CancellationToken cancellationToken = default);
}