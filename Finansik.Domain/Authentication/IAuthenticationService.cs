namespace Finansik.Domain.Authentication;

public interface IAuthenticationService
{
    Task<(bool success, string authToken)> SignIn(SignInCredentials credentials, CancellationToken cancellationToken);

    Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken);
}