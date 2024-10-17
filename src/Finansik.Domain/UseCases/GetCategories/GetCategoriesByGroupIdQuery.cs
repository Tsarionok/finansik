using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetCategories;

public sealed record GetCategoriesByGroupIdQuery(Guid GroupId) : 
    DefaultMonitoredRequest("categories-by-group-id.fetched"), IRequest<IEnumerable<Category>>;