using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateCategory;

public interface ICreateCategoryUseCase : IUseCase<CreateCategoryCommand, Category>;