using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroup;

internal class GetGroupUseCase : IRequestHandler<GetGroupQuery, Group>
{
    public async Task<Group> Handle(GetGroupQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}