using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Models;
using Finansik.Domain.Storages;
using MediatR;

namespace Finansik.Domain.UseCases.DeleteCategory;

internal class DeleteCategoryUseCase(
    IIntentionManager intentionManager,
    IDeleteCategoryStorage storage) : IRequestHandler<DeleteCategoryCommand, Category>
{
    public async Task<Category> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(CategoryIntention.Delete);
        await storage.ThrowIfCategoryNotExistsAsync(command.CategoryId, cancellationToken);
        
        return await storage.DeleteCategory(command.CategoryId, cancellationToken);
    }
}