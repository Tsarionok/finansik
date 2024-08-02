using Finansik.Domain.Authentication;
using Finansik.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace Finansik.Domain.UseCases.SignOn;

internal class SignOnUseCase(
    IValidator<SignOnCommand> validator,
    ISignOnStorage storage,
    IPasswordManager passwordManager) : IRequestHandler<SignOnCommand, IIdentity>
{
    public async Task<IIdentity> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        // TODO: remake to ThrowIfLoginAlreadyUsed
        if (await storage.IsLoginAlreadyUsed(command.Login, cancellationToken))
        {
            throw new LoginAlreadyUsedException(command.Login);
        }
        
        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);
        
        // TODO: fix Guid.Empty as session ID
        return new User(userId, Guid.Empty);
    }
}