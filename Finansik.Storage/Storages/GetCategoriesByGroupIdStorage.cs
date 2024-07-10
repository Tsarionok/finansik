using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetCategories;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

internal class GetCategoriesByGroupIdStorage (
    FinansikDbContext dbContext,
    IMapper mapper) : IGetCategoriesByGroupIdStorage
{
    public async Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken) => 
        await dbContext.Groups.AnyAsync(g => g.Id == groupId, cancellationToken);

    public async Task<IEnumerable<Category>> GetCategoriesByGroupId(Guid groupId, CancellationToken cancellationToken) =>
        await dbContext.Categories
            .ProjectTo<Category>(mapper.ConfigurationProvider)
            .Where(c => c.GroupId == groupId)
            .ToArrayAsync(cancellationToken);
}