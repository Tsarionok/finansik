using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroupMembers;

public interface IGetGroupMembersStorage
{
    Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken);

    Task<GroupMembers> FindUsersByGroupId(Guid groupId, CancellationToken cancellationToken);
}