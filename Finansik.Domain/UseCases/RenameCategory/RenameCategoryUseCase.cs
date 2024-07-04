using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;

namespace Finansik.Domain.UseCases.RenameCategory;

public class RenameCategoryUseCase(
    IRenameCategoryStorage storage,
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IRenameCategoryUseCase
{
    public async Task<Category> Execute(Guid categoryId, string nextName, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(CategoryIntention.Rename);
        
        var categoryExists = await storage.IsCategoryExists(categoryId, cancellationToken);

        if (!categoryExists)
        {
            throw new CategoryNotFoundException(categoryId);
        }

        return await storage.RenameCategory(categoryId, nextName, cancellationToken);
    }
}