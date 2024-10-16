using Finansik.Domain.Exceptions;
using MediatR;

namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public class RemoveMemberFromGroupUseCase(
    IRemoveMemberFromGroupStorage storage
    ) : IRequestHandler<RemoveMemberFromGroupCommand, Guid>
{
    public async Task<Guid> Handle(RemoveMemberFromGroupCommand command, CancellationToken cancellationToken)
    {
        if (!await storage.IsGroupExists(command.GroupId, cancellationToken))
            throw new GroupNotFoundException(command.GroupId);

        if (!await storage.IsUserExists(command.UserId, cancellationToken))
            throw new UserNotFoundException(command.UserId);

        await storage.ResetGroupIdForUser(command.UserId, command.GroupId, cancellationToken);
        
        return command.UserId;
    }
}