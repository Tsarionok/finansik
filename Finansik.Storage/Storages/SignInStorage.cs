using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.SignIn;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public class SignInStorage(
    FinansikDbContext dbContext, 
    IMapper mapper) : ISignInStorage
{
    public async Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken) =>
        await dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Guid> CreateSession(Guid userId, DateTimeOffset expireAt, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}