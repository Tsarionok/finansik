namespace Finansik.Domain.UseCases.RenameCategory;

public record RenameCategoryCommand(Guid CategoryId, string NextName) : ICommand;