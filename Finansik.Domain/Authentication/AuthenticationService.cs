using Finansik.Domain.Authentication.Cryptography;
using Microsoft.Extensions.Options;

namespace Finansik.Domain.Authentication;

internal class AuthenticationService(
    ISymmetricDecryptor decryptor,
    IOptions<AuthenticationConfiguration> options)
    : IAuthenticationService
{
    private readonly AuthenticationConfiguration _configuration = options.Value;

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        var userIdString = await decryptor.Decrypt(authToken, _configuration.Key, cancellationToken);
        // TODO: verify user identifier
        return new User(Guid.Parse(userIdString));
    }
}