using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.RenameCategory;

public sealed record RenameCategoryCommand(Guid CategoryId, string NextName) : 
    DefaultMonitoredRequest("categories.renamed"), IRequest<Category>;