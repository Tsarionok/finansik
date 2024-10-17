using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupMembers;

public record GetGroupMembersQuery(Guid GroupId) : 
    DefaultMonitoredRequest("group.members.fetched"), IRequest<GroupMembers>;