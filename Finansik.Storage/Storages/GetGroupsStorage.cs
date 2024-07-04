using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroups;

namespace Finansik.Storage.Storages;

public class GetGroupsStorage : IGetGroupsStorage
{
    public Task<IEnumerable<Group>> GetGroupsByUserId(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}