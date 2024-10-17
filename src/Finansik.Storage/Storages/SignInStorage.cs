using AutoMapper;
using AutoMapper.QueryableExtensions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.SignIn;
using Microsoft.EntityFrameworkCore;
using Session = Finansik.Storage.Entities.Session;

namespace Finansik.Storage.Storages;

public sealed class SignInStorage(
    FinansikDbContext dbContext, 
    IGuidFactory guidFactory,
    IMapper mapper) : ISignInStorage
{
    public async Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken) =>
        await dbContext.Users
            .Where(u => u.Login.Equals(login))
            .ProjectTo<RecognisedUser>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Guid> CreateSession(Guid userId, DateTimeOffset expireAt, CancellationToken cancellationToken)
    {
        var sessionId = guidFactory.Create();
        await dbContext.Sessions.AddAsync(new Session
        {
            Id = sessionId,
            UserId = userId,
            ExpiresAt = expireAt
        }, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return sessionId;
    }
}