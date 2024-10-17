using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.AddMemberToGroup;

public interface IAddMemberToGroupStorage
{
    Task<bool> IsUserExists(Guid userId, CancellationToken cancellationToken);

    Task<bool> IsGroupExists(Guid groupId, CancellationToken cancellationToken);
    
    Task AddUserToGroup(Guid groupId, Guid userId, CancellationToken cancellationToken);

    Task<GroupMembers> GetUsers(Guid groupId, CancellationToken cancellationToken);
}