using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.AddMemberToGroup;

public sealed record AddMemberToGroupCommand(Guid GroupId, Guid UserId) :
    DefaultMonitoredRequest("group.members.add"), IRequest<GroupMembers>;