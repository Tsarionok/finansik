using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class AuthenticationStorage(
    FinansikDbContext dbContext, 
    IMapper mapper) : IAuthenticationStorage
{
    public async Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken) =>
        await dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
}