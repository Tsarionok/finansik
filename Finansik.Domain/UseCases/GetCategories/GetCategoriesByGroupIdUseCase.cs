using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Storage;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Domain.UseCases.GetCategories;

public class GetCategoriesByGroupIdUseCase(FinansikDbContext dbContext) : IGetCategoriesByGroupIdUseCase
{
    public async Task<IEnumerable<Category>> Execute(Guid groupId, CancellationToken cancellationToken)
    {
        if (!await dbContext.Groups.AnyAsync(g => g.Id == groupId, cancellationToken))
        {
            throw new GroupNotFoundException(groupId);
        }

        var categories = await dbContext.Categories.Where(c => c.GroupId == groupId).ToListAsync(cancellationToken);
        
        return categories.Select(c => new Category
        {
            Id = c.Id,
            GroupId = c.GroupId,
            Name = c.Name,
            Icon = c.Icon
        });
    }
}