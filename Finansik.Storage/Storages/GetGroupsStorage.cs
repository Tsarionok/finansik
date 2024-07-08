using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroups;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

internal class GetGroupsStorage(FinansikDbContext dbContext) : IGetGroupsStorage
{
    public async Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId, CancellationToken cancellationToken) =>
        // TODO: implement filtrating by user id
        await dbContext.Groups.Select(g => new Group
        {
            Id = g.Id,
            Name = g.Name,
            Icon = g.Icon
        }).ToArrayAsync(cancellationToken);
}