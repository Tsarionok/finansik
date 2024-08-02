using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Models;
using Finansik.Domain.Storages;
using FluentValidation;
using MediatR;

namespace Finansik.Domain.UseCases.DeleteCategory;

internal class DeleteCategoryUseCase(
    IValidator<DeleteCategoryCommand> validator,
    IIntentionManager intentionManager,
    IDeleteCategoryStorage storage) : IRequestHandler<DeleteCategoryCommand, Category>
{
    public async Task<Category> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(CategoryIntention.Delete);
        await storage.ThrowIfCategoryNotExistsAsync(command.CategoryId, cancellationToken);
        
        return await storage.DeleteCategory(command.CategoryId, cancellationToken);
    }
}