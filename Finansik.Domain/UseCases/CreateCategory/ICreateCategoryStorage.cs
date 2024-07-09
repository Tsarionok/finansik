using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public interface ICreateCategoryStorage
{
    Task<Category> CreateCategory(string name, Guid groupId, Guid userId, string? icon, CancellationToken cancellationToken);

    Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken);
}