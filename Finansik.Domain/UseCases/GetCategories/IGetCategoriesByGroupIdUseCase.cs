using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetCategories;

public interface IGetCategoriesByGroupIdUseCase : IUseCase<GetCategoriesByGroupIdCommand, IEnumerable<Category>>;