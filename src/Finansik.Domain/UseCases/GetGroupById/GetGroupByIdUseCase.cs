using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupById;

internal sealed class GetGroupByIdUseCase
    (IGetGroupByIdStorage storage) : IRequestHandler<GetGroupByIdQuery, Group>
{
    public async Task<Group> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
    {
        var group = await storage.FindGroup(query.Id, cancellationToken);

        if (group is null)
            throw new GroupNotFoundException(query.Id);

        return group;
    }
}