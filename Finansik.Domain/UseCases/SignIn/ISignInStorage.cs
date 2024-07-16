using Finansik.Domain.Models;

namespace Finansik.Domain.UseCases.SignIn;

public interface ISignInStorage
{
    Task<RecognisedUser?> FindUser(string login, CancellationToken cancellationToken);
}