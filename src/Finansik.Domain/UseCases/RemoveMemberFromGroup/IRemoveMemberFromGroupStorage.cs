namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public interface IRemoveMemberFromGroupStorage
{
    Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken);

    Task<bool> IsGroupExists(Guid group, CancellationToken cancellationToken);

    Task ResetGroupIdForUser(Guid userId, Guid groupId, CancellationToken cancellationToken);
}