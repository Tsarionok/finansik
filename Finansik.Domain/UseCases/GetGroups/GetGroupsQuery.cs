using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroups;

public sealed record GetGroupsQuery() : DefaultMonitoredRequest("groups.fetched"), IRequest<IEnumerable<Group>>;