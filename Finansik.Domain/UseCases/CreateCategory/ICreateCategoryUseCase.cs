using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public interface ICreateCategoryUseCase
{
    Task<Category> Execute(string name, Guid groupId, string? icon = null, CancellationToken cancellationToken = default);
}