using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroup;

public record GetGroupQuery() : DefaultMonitoredRequest("categories.fetched"), IRequest<Group>;