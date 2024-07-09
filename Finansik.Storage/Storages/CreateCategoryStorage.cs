using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

internal class CreateCategoryStorage(
    FinansikDbContext dbContext, 
    IGuidFactory guidFactory,
    IMapper mapper) : ICreateCategoryStorage
{
    public Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken) => 
        dbContext.Groups.AnyAsync(g => g.Id == groupId, cancellationToken);

    public async Task<Category> CreateCategory(string name, Guid groupId, Guid userId, string? icon, 
        CancellationToken cancellationToken)
    {
        var categoryId = guidFactory.Create();
        var category = new Entities.Category
        {
            Id = categoryId,
            Icon = icon,
            Name = name,
            Creator = userId,
            GroupId = groupId
        };
        
        await dbContext.Categories.AddAsync(category, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return await dbContext.Categories
            .ProjectTo<Category>(mapper.ConfigurationProvider)
            .FirstAsync(c => c.Id == categoryId, cancellationToken);
    }
}