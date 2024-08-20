using Finansik.Domain.UseCases.SignOut;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Storages;

public sealed class SignOutStorage(FinansikDbContext dbContext) : ISignOutStorage
{
    public async Task RemoveSession(Guid sessionId, CancellationToken cancellationToken)
    {
        var deletableSession = dbContext.Sessions.Where(session => session.Id == sessionId);
        await deletableSession.ExecuteDeleteAsync(cancellationToken);
    }
}