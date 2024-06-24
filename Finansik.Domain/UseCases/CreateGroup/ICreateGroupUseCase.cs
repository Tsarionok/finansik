using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.CreateGroup;

public interface ICreateGroupUseCase
{
    Task<Group> Execute(string name, string icon, CancellationToken cancellationToken);
}