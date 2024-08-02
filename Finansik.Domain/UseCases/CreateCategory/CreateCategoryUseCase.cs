using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using FluentValidation;
using MediatR;

namespace Finansik.Domain.UseCases.CreateCategory;

internal class CreateCategoryUseCase(
    IValidator<CreateCategoryCommand> validator,
    ICreateCategoryStorage createCategoryStorage, 
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : IRequestHandler<CreateCategoryCommand, Category>
{
    public async Task<Category> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var (groupId, name, icon) = command;
        
        intentionManager.ThrowIfForbidden(CategoryIntention.Create);
        
        var groupExists = await createCategoryStorage.IsGroupExists(groupId, cancellationToken);

        if (!groupExists)
        {
            throw new GroupNotFoundException(groupId);
        }

        return await createCategoryStorage.CreateCategory(name, groupId, identityProvider.Current.UserId, icon, cancellationToken);
    }
}