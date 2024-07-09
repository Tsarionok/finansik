using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;

namespace Finansik.Domain.UseCases.RenameCategory;

internal class RenameCategoryUseCase(
    IRenameCategoryStorage storage,
    IIntentionManager intentionManager) : IRenameCategoryUseCase
{
    public async Task<Category> ExecuteAsync(RenameCategoryCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(CategoryIntention.Rename);
        
        var categoryExists = await storage.IsCategoryExists(command.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            throw new CategoryNotFoundException(command.CategoryId);
        }

        return await storage.RenameCategory(command.CategoryId, command.NextName, cancellationToken);
    }
}