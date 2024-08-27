using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroup;

public interface IGetGroupStorage
{
    Task<Group> FindGroup(Guid id, CancellationToken cancellationToken);
}