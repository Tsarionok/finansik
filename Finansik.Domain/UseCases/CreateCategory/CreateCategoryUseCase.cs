using Finansik.Domain.Exceptions;
using Finansik.Domain.Identity;
using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public class CreateCategoryUseCase(ICreateCategoryStorage createCategoryStorage, IIdentityProvider identityProvider) : ICreateCategoryUseCase
{
    public async Task<Category> Execute(string name, Guid groupId, string? icon = null,
        CancellationToken cancellationToken = default)
    {
        var groupExists = await createCategoryStorage.IsGroupExists(groupId, cancellationToken);

        if (!groupExists)
        {
            throw new GroupNotFoundException(groupId);
        }

        return await createCategoryStorage.CreateCategory(name, groupId, identityProvider.Current.UserId, icon, cancellationToken);
    }
}