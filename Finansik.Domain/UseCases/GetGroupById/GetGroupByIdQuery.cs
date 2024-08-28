using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupById;

public sealed record GetGroupByIdQuery(Guid Id) : DefaultMonitoredRequest("groups.fetched"), IRequest<Group>;