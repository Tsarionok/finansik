using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Storage;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Domain.UseCases.CreateCategory;

public class CreateCategoryUseCase(IGuidFactory guidFactory, FinansikDbContext dbContext) : ICreateCategoryUseCase
{
    public async Task<Category> Execute(string name, Guid groupId, string? icon = null,
        CancellationToken cancellationToken = default)
    {
        var groupExists = await dbContext.Groups.AnyAsync(g => g.Id.Equals(groupId), cancellationToken);

        if (!groupExists)
        {
            throw new GroupNotFoundException(groupId);
        }

        var addedCategory = (await dbContext.Categories.AddAsync(new Storage.Entities.Category
        {
            Id = guidFactory.Create(),
            Name = name,
            GroupId = groupId,
            Icon = icon
        }, cancellationToken)).Entity;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Category
        {
            Id = addedCategory.Id,
            GroupId = addedCategory.GroupId,
            Icon = addedCategory.Icon,
            Name = addedCategory.Name
        };
    }
}