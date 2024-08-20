using Finansik.Domain.Models;
using Finansik.Domain.UseCases.RenameCategory;

namespace Finansik.Storage.Storages;

internal sealed class RenameCategoryStorage : IRenameCategoryStorage
{
    public Task<bool> IsCategoryExists(Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Category> RenameCategory(Guid categoryId, string nextName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}