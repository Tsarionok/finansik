using Finansik.Domain.Authentication;
using Finansik.Domain.Authentication.Cryptography;
using Finansik.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Options;

namespace Finansik.Domain.UseCases.SignIn;

internal class SignInUseCase(
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor,
    IOptions<AuthenticationConfiguration> options) : 
        IRequestHandler<SignInCommand, (IIdentity identity, string token)>
{
    private readonly AuthenticationConfiguration _configuration = options.Value;
    
    public async Task<(IIdentity identity, string token)> Handle(
        SignInCommand command, 
        CancellationToken cancellationToken)
    {
        var recognizedUser = await storage.FindUser(command.Login, cancellationToken);
        if (recognizedUser is null)
            throw new UserNotRecognizedException(command.Login);
        
        passwordManager.ThrowIfPasswordNotMatched(
            command.Password, recognizedUser.Salt, recognizedUser.PasswordHash);

        // TODO: remake const expiry date
        var sessionId = await storage.CreateSession(
            recognizedUser.UserId, DateTimeOffset.UtcNow.AddHours(1), cancellationToken);
        
        var token = await encryptor.Encrypt(sessionId.ToString(), _configuration.Key, cancellationToken);

        return (new User(recognizedUser.UserId, sessionId), token);
    }
}