using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public class CreateCategoryUseCase(
    ICreateCategoryStorage createCategoryStorage, 
    IIdentityProvider identityProvider,
    IIntentionManager intentionManager) : ICreateCategoryUseCase
{
    public async Task<Category> Execute(string name, Guid groupId, string? icon = null,
        CancellationToken cancellationToken = default)
    {
        intentionManager.ThrowIfForbidden(CategoryIntention.Create);
        
        var groupExists = await createCategoryStorage.IsGroupExists(groupId, cancellationToken);

        if (!groupExists)
        {
            throw new GroupNotFoundException(groupId);
        }

        return await createCategoryStorage.CreateCategory(name, groupId, identityProvider.Current.UserId, icon, cancellationToken);
    }
}