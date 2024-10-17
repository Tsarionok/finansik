using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.DeleteCategory;

namespace Finansik.Domain.Storages;

internal static class CategoryStorageExtensions
{
    internal static async Task ThrowIfCategoryNotExistsAsync(
        this IDeleteCategoryStorage storage, Guid categoryId, CancellationToken cancellationToken)
    {
        if (!await storage.IsCategoryExists(categoryId, cancellationToken))
        {
            throw new CategoryNotFoundException(categoryId);
        }
    }
}