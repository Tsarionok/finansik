using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroups;

public interface IGetGroupsUseCase
{
    Task<IEnumerable<Group>> Execute(CancellationToken cancellationToken);
}