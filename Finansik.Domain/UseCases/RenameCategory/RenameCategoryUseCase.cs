using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.RenameCategory;

internal class RenameCategoryUseCase(
    IRenameCategoryStorage storage,
    IIntentionManager intentionManager) : IRequestHandler<RenameCategoryCommand, Category>
{
    public async Task<Category> Handle(RenameCategoryCommand command, CancellationToken cancellationToken)
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