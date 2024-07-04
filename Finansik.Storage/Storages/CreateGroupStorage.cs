using Finansik.Domain;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateGroup;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class CreateGroupStorage(
    FinansikDbContext dbContext,
    IGuidFactory guidFactory) : ICreateGroupStorage
{
    public async Task<Group> CreateGroup(string name, Guid creator, string icon, CancellationToken cancellationToken)
    {
        var groupId = guidFactory.Create();
        
        await dbContext.Groups.AddAsync(new Entities.Group
        {
            Id = groupId,
            Creator = creator,
            Icon = icon,
            Name = name
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return await dbContext.Groups
            .Select(g => new Group
            {
                Id = g.Id,
                Icon = g.Icon,
                Name = g.Name
            })
            .FirstAsync(g => g.Id == groupId, cancellationToken);
    }
}