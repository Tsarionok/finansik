using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Category>;