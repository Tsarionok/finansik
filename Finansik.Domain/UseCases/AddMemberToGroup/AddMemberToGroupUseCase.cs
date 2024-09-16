using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.AddMemberToGroup;

internal sealed class AddMemberToGroupUseCase(
    IAddMemberToGroupStorage storage
    ) : IRequestHandler<AddMemberToGroupCommand, GroupMembers>
{
    public async Task<GroupMembers> Handle(AddMemberToGroupCommand command, CancellationToken cancellationToken)
    {
        if (!await storage.IsUserExists(command.UserId, cancellationToken))
            throw new UserNotFoundException(command.UserId);

        if (!await storage.IsGroupExists(command.GroupId, cancellationToken))
            throw new GroupNotFoundException(command.GroupId);

        await storage.AddUserToGroup(command.GroupId, command.UserId, cancellationToken);

        return await storage.GetUsers(command.GroupId, cancellationToken);
    }
}