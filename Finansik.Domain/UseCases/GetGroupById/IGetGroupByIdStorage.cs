using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroupById;

public interface IGetGroupByIdStorage
{
    Task<Group> FindGroup(Guid id, CancellationToken cancellationToken);
}