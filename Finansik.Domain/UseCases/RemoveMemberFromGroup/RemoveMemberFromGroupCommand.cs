using MediatR;

namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public sealed record RemoveMemberFromGroupCommand (Guid GroupId, Guid UserId) : IRequest, IRequest<Guid>;