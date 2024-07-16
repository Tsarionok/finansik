using Finansik.Domain.Authentication;

namespace Finansik.Domain.UseCases.SignOn;

public interface ISignOnUseCase
{
    Task<IIdentity> Execute(SignOnCommand command, CancellationToken cancellationToken);
}