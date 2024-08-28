using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroup;

public interface IGetGroupByIdStorage
{
    Task<Group> FindGroup(Guid id, CancellationToken cancellationToken);
}