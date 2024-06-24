using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Storage;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Domain.UseCases.RenameCategory;

public class RenameCategoryUseCase(FinansikDbContext dbContext) : IRenameCategoryUseCase
{
    public async Task<Category> Execute(Guid categoryId, string nextName, CancellationToken cancellationToken)
    {
        if (!await dbContext.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken))
            throw new CategoryNotFoundException(categoryId);

        // TODO: remove extra db request
        var category = await dbContext.Categories.FirstAsync(c => c.Id == categoryId, cancellationToken);
        category.Name = nextName;
        dbContext.Categories.Update(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Category
        {
            Id = category.Id,
            Name = category.Name,
            GroupId = category.GroupId,
            Icon = category.Icon
        };
    }
}