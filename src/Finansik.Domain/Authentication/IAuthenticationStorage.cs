using Finansik.Domain.Models;

namespace Finansik.Domain.Authentication;

public interface IAuthenticationStorage
{
    Task<Session?> FindSession(Guid sessionId, CancellationToken cancellationToken);
}