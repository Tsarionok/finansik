using Finansik.Domain.Authentication;
using FluentValidation;


namespace Finansik.Domain.UseCases.SignOn;

internal class SignOnUseCase(
    IValidator<SignOnCommand> validator,
    ISignOnStorage storage,
    IPasswordManager passwordManager) : ISignOnUseCase
{
    public async Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        if (await storage.IsLoginAlreadyUsed(command.Login, cancellationToken))
        {
            throw new Exception();
        }
        
        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);
        
        return new User(userId);
    }
}