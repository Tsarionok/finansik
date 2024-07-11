using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Finansik.Domain.Authentication;

internal class AuthenticationService(
    IAuthenticationStorage storage,
    ISecurityManager securityManager,
    IOptions<AuthenticationConfiguration> options)
    : IAuthenticationService
{
    private readonly Lazy<TripleDES> _tripleDesService = new(TripleDES.Create);
    private readonly AuthenticationConfiguration _configuration = options.Value;

    public async Task<(bool, string)> SignIn(SignInCredentials credentials, CancellationToken cancellationToken)
    {
        var recognisedUser = await storage.FindUser(credentials.Login, cancellationToken);
        if (recognisedUser is null)
        {
            throw new Exception("User not found");
        }

        var success = securityManager.ComparePasswords(credentials.Password, recognisedUser.Salt, recognisedUser.PasswordHash);
        var userIdBytes = recognisedUser.UserId.ToByteArray();

        using var encryptedStream = new MemoryStream();
        var key = Convert.FromBase64String(_configuration.Key);
        var iv = Convert.FromBase64String(_configuration.Iv);
        await using (var stream = new CryptoStream(
                         encryptedStream,
                         _tripleDesService.Value.CreateEncryptor(key, iv),
                         CryptoStreamMode.Write))
        {
            await stream.WriteAsync(userIdBytes, cancellationToken);
        }
        
        return (success, Convert.ToBase64String(encryptedStream.ToArray()));
    }

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        using var decryptedStream = new MemoryStream();
        var key = Convert.FromBase64String(_configuration.Key);
        var iv = Convert.FromBase64String(_configuration.Iv);

        await using (var stream = new CryptoStream(
                         decryptedStream,
                         _tripleDesService.Value.CreateDecryptor(key, iv),
                         CryptoStreamMode.Write))
        {
            var encryptedBytes = Convert.FromBase64String(authToken);
            await stream.WriteAsync(encryptedBytes, cancellationToken);
        }

        var userId = new Guid(decryptedStream.ToArray());
        return new User(userId);
    }
}