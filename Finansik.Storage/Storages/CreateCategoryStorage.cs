using Finansik.Domain;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class CreateCategoryStorage(FinansikDbContext dbContext, IGuidFactory guidFactory) : ICreateCategoryStorage
{
    public Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken) => 
        dbContext.Groups.AnyAsync(g => g.Id == groupId, cancellationToken);

    public async Task<Category> CreateCategory(string name, Guid groupId, Guid userId, string? icon, 
        CancellationToken cancellationToken)
    {
        var categoryId = guidFactory.Create();
        await dbContext.Categories.AddAsync(new Entities.Category
        {
            Id = categoryId,
            Icon = icon,
            Name = name,
            Creator = userId,
            GroupId = groupId
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return await dbContext.Categories
            .Select(c => new Category
            {
                Id = c.Id,
                GroupId = c.GroupId,
                Icon = c.Icon,
                Name = c.Name
            })
            .FirstAsync(c => c.Id == categoryId, cancellationToken);
    }
}