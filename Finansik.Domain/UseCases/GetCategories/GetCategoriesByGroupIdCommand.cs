namespace Finansik.Domain.UseCases.GetCategories;

public record GetCategoriesByGroupIdCommand(Guid GroupId) : ICommand;