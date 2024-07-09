namespace Finansik.Domain.UseCases.CreateGroup;

public record CreateGroupCommand(string Name, string? Icon) : ICommand;