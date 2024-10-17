using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupMembers;

public class GetGroupMembersUseCase(IGetGroupMembersStorage storage) : 
    IRequestHandler<GetGroupMembersQuery, GroupMembers>
{
    public async Task<GroupMembers> Handle(GetGroupMembersQuery query, CancellationToken cancellationToken)
    {
        if (!await storage.IsGroupExists(query.GroupId, cancellationToken))
            throw new GroupNotFoundException(query.GroupId);
        
        return await storage.FindUsersByGroupId(query.GroupId, cancellationToken);
    }
}