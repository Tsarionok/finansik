using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Authentication;
using Finansik.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public sealed class AuthenticationStorage(
    FinansikDbContext dbContext,
    IMapper mapper
    ) : IAuthenticationStorage
{
    public async Task<Session?> FindSession(Guid sessionId, CancellationToken cancellationToken) 
        => await dbContext.Sessions
            .Where(session => session.Id == sessionId)
            .ProjectTo<Session>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}