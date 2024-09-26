using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupById;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupMembers;

public class GetGroupMembersUseCase(IGetGroupByIdStorage storage) : 
    IRequestHandler<GetGroupMembersQuery, IEnumerable<GroupMembers>>
{
    public Task<IEnumerable<GroupMembers>> Handle(GetGroupMembersQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}