using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetCategories;

public record GetCategoriesByGroupIdQuery(Guid GroupId) : IRequest<IEnumerable<Category>>;