using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public interface ICreateCategoryStorage
{
    Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken);

    Task<Category> CreateCategory(string name, Guid groupId, string? icon, CancellationToken cancellationToken);
}