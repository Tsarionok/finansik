using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroups;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Finansik.Storage.Storages;

internal class GetGroupsStorage(
    FinansikDbContext dbContext,
    IMemoryCache cache) : IGetGroupsStorage
{
    public async Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId, CancellationToken cancellationToken) =>
        (await cache.GetOrCreateAsync<Group[]>(nameof(GetGroupsByUserId), f =>
        {
            // TODO: implement filtrating by user id
            return dbContext.Groups.Select(g => new Group
            {
                Id = g.Id,
                Name = g.Name,
                Icon = g.Icon
            }).ToArrayAsync(cancellationToken);
        }))!;
}