using Finansik.Domain.Models;
using Finansik.Domain.UseCases.DeleteCategory;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class DeleteCategoryStorage(FinansikDbContext dbContext) : IDeleteCategoryStorage
{
    public async Task<Category> DeleteCategory(Guid categoryId, CancellationToken cancellationToken)
    {
        var filteredCategories = dbContext.Categories.Where(c => c.Id == categoryId);
        var category = await filteredCategories.FirstAsync(cancellationToken);
        await filteredCategories.ExecuteDeleteAsync(cancellationToken);

        return new Category
        {
            Id = category.Id,
            GroupId = category.GroupId,
            Icon = category.Icon,
            Name = category.Name
        };
    }

    public async Task<bool> IsCategoryExists(Guid categoryId, CancellationToken cancellationToken) => 
        await dbContext.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
}