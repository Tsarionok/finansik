namespace Finansik.Domain.UseCases.SignOn;

public interface ISignOnStorage
{
    Task<bool> IsLoginAlreadyUsed(string login, CancellationToken cancellationToken);

    Task<Guid> CreateUser(string login, byte[] salt, byte[] passwordHash, CancellationToken cancellationToken);
}