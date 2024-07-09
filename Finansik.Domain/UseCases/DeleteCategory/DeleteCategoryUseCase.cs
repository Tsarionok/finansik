using Finansik.Domain.Authorization;
using Finansik.Domain.Models;
using Finansik.Domain.Storages;
using Finansik.Domain.UseCases.CreateCategory;
using FluentValidation;

namespace Finansik.Domain.UseCases.DeleteCategory;

internal class DeleteCategoryUseCase(
    IValidator<DeleteCategoryCommand> validator,
    IIntentionManager intentionManager,
    IDeleteCategoryStorage storage) : IDeleteCategoryUseCase
{
    public async Task<Category> ExecuteAsync(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(CategoryIntention.Delete);
        await storage.ThrowIfCategoryNotExistsAsync(command.CategoryId, cancellationToken);
        
        return await storage.DeleteCategory(command.CategoryId, cancellationToken);
    }
}