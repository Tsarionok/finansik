using Finansik.Domain.Models;
using Finansik.Storage;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Domain.UseCases.GetGroups;

public class GetGroupsUseCase(FinansikDbContext dbContext) : IGetGroupsUseCase
{
    public async Task<IEnumerable<Group>> Execute(CancellationToken cancellationToken) =>
        await dbContext.Groups.Select(g => new Group
        {
            Id = g.Id,
            Name = g.Name,
            Icon = g.Icon
        }).ToListAsync(cancellationToken);
}