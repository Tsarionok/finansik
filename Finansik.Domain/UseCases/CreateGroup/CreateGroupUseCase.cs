using Finansik.Domain.Models;
using Finansik.Storage;

namespace Finansik.Domain.UseCases.CreateGroup;

public class CreateGroupUseCase(FinansikDbContext dbContext, IGuidFactory guidFactory) : ICreateGroupUseCase
{
    public async Task<Group> Execute(string name, string icon, CancellationToken cancellationToken)
    {
        var addedGroup = (await dbContext.Groups.AddAsync(new Storage.Entities.Group
        {
            Id = guidFactory.Create(),
            Name = name,
            Icon = icon
        }, cancellationToken)).Entity;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Group
        {
            Id = addedGroup.Id,
            Name = addedGroup.Name,
            Icon = addedGroup.Icon
        };
    }
}