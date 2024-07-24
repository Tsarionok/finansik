using System.Security.Cryptography;
using Finansik.Domain.Authentication.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finansik.Domain.Authentication;

internal class AuthenticationService(
    ISymmetricDecryptor decryptor,
    IAuthenticationStorage storage,
    ILogger<AuthenticationService> logger,
    IOptions<AuthenticationConfiguration> options)
    : IAuthenticationService
{
    private readonly AuthenticationConfiguration _configuration = options.Value;

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        string sessionIdString;
        try
        {
            sessionIdString = await decryptor.Decrypt(authToken, _configuration.Key, cancellationToken);
        }
        catch (CryptographicException cryptographicException)
        {
            logger.LogWarning(
                cryptographicException, 
                "Cannot decrypt auth token, maybe someone is trying to forge it");
            return User.Guest;
        }
        
        if (!Guid.TryParse(sessionIdString, out var sessionId))
        {
            logger.LogWarning("Cannot parse token to GUID format");
            return User.Guest;
        }

        var session = await storage.FindSession(sessionId, cancellationToken);

        if (session is null)
        {
            return User.Guest;
        }

        if (session.ExpiresAt < DateTimeOffset.UtcNow)
        {
            return User.Guest;
        }
        
        return new User(session.UserId, sessionId);
    }
}