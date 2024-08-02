using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.RenameCategory;

public record RenameCategoryCommand(Guid CategoryId, string NextName) : IRequest<Category>;