using Finansik.Domain.Authentication;
using Finansik.Domain.Models;

namespace Finansik.Storage.Storages;

public class AuthenticationStorage : IAuthenticationStorage
{
    public Task<Session?> FindSession(Guid sessionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}