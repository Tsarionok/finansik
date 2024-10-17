using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.AddMemberToGroup;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class AddMemberToGroupStorage (
    FinansikDbContext dbContext,
    IMapper mapper
    ): IAddMemberToGroupStorage
{
    public Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken) =>
        dbContext.Users.AnyAsync(user => user.Id == userId, cancellationToken);

    public Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken) =>
        dbContext.Groups.AnyAsync(group => group.Id == groupId, cancellationToken);

    public Task AddUserToGroup(Guid groupId, Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<GroupMembers> GetUsers(Guid groupId, CancellationToken cancellationToken)
    {
        var users = dbContext.Users.Where(user => user.Groups!.Any(group => group.Id == groupId));

        return new GroupMembers
        {
            GroupId = groupId,
            Members = await users.ProjectTo<GroupMembers.GroupUser>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}