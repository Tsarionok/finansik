using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public sealed record RemoveMemberFromGroupCommand (Guid GroupId, Guid UserId) 
    : DefaultMonitoredRequest("group.members.remove"), IRequest, IRequest<Guid>;