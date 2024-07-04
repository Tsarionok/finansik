using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.GetGroups;

public interface IGetGroupsStorage
{
    Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId, CancellationToken cancellationToken);
}