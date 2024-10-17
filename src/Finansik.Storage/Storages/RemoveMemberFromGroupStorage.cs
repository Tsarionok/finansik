using Finansik.Domain.UseCases.RemoveMemberFromGroup;

namespace Finansik.Storage.Storages;

public class RemoveMemberFromGroupStorage : IRemoveMemberFromGroupStorage
{
    public Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsGroupExists(Guid group, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task ResetGroupIdForUser(Guid userId, Guid groupId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}