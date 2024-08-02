using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : 
    DefaultMonitoredRequest("categories.deleted"), IRequest<Category>;