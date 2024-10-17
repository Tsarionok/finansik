using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupMembers;

namespace Finansik.Storage.Storages;

public class GetGroupMembersStorage : IGetGroupMembersStorage
{
    public Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GroupMembers> FindUsersByGroupId(Guid groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}