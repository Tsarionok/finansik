using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupById;

internal sealed class GetGroupByIdUseCase
    (IGetGroupByIdStorage storage) : IRequestHandler<GetGroupByIdQuery, Group>
{
    public Task<Group> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken) => 
        storage.FindGroup(query.Id, cancellationToken);
}