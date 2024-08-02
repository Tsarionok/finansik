using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.CreateCategory;

public record CreateCategoryCommand(Guid GroupId, string Name, string? Icon) : 
    DefaultMonitoredRequest("categories.created"), IRequest<Category>;