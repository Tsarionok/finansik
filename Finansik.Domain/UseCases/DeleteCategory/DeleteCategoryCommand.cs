namespace Finansik.Domain.UseCases.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : ICommand;