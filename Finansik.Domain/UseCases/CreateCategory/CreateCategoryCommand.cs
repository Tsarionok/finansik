using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.CreateCategory;

public record CreateCategoryCommand(Guid GroupId, string Name, string? Icon) : IRequest<Category>;