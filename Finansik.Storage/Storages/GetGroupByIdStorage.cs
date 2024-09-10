using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupById;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class GetGroupByIdStorage(
    FinansikDbContext dbContext,
    IMapper mapper) : IGetGroupByIdStorage
{
    public Task<Group> FindGroup(Guid id, CancellationToken cancellationToken) =>
        dbContext.Groups.Where(g => g.Id == id)
            .ProjectTo<Group>(mapper.ConfigurationProvider)
            .FirstAsync(cancellationToken);
}