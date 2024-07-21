using Finansik.Domain.Authentication;

namespace Finansik.Domain.UseCases.SignOn;

public interface ISignOnUseCase
{
    Task<Guid> Execute(SignOnCommand command, CancellationToken cancellationToken);
}