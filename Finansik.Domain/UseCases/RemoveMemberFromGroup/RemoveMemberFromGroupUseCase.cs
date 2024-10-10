using MediatR;

namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public class RemoveMemberFromGroupUseCase : IRequestHandler<RemoveMemberFromGroupCommand, Guid>
{
    public Task<Guid> Handle(RemoveMemberFromGroupCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}