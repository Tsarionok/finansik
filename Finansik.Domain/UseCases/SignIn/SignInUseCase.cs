using Finansik.Domain.Authentication;
using Finansik.Domain.Authentication.Cryptography;
using Finansik.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Finansik.Domain.UseCases.SignIn;

internal class SignInUseCase(
    IValidator<SignInCommand> validator, 
    ISignInStorage storage,
    IPasswordManager passwordManager,
    ISymmetricEncryptor encryptor,
    IOptions<AuthenticationConfiguration> options) : ISignInUseCase
{
    private readonly AuthenticationConfiguration _configuration = options.Value;
    
    public async Task<(IIdentity identity, string token)> Execute(
        SignInCommand command, 
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        // TODO: remake to ThrowIfUserNotRecognizedAsync
        var recognizedUser = await storage.FindUser(command.Login, cancellationToken);
        if (recognizedUser is null)
            throw new UserNotRecognizedException(command.Login);

        // TODO: remake to ThrowIfPasswordNotMatched
        var passwordsMatch = passwordManager.ComparePasswords(
            command.Password, recognizedUser.Salt, recognizedUser.PasswordHash);

        if (!passwordsMatch)
            throw new PasswordNotMatchedException();
        
        var token = await encryptor.Encrypt(recognizedUser.UserId.ToString(), _configuration.Key, cancellationToken);

        return (new User(recognizedUser.UserId), token);
    }
}