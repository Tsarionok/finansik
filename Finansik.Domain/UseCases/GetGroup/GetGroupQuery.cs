using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroup;

public sealed record GetGroupQuery(Guid Id) : DefaultMonitoredRequest("groups.fetched"), IRequest<Group>;