using Finansik.Domain.Authentication;
using Finansik.Domain.Exceptions;
using FluentValidation;


namespace Finansik.Domain.UseCases.SignOn;

internal class SignOnUseCase(
    IValidator<SignOnCommand> validator,
    ISignOnStorage storage,
    IPasswordManager passwordManager) : ISignOnUseCase
{
    public async Task<Guid> Execute(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        // TODO: remake to ThrowIfLoginAlreadyUsed
        if (await storage.IsLoginAlreadyUsed(command.Login, cancellationToken))
        {
            throw new LoginAlreadyUsedException(command.Login);
        }
        
        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);
        
        return userId;
    }
}