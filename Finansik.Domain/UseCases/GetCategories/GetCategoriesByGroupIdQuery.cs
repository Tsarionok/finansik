using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.GetCategories;

public record GetCategoriesByGroupIdQuery(Guid GroupId) : 
    DefaultMonitoredRequest("categories-by-group-id.fetched"), IRequest<IEnumerable<Category>>;