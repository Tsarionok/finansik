namespace Finansik.Domain.Authentication;

public interface IIdentityProvider
{
    public IIdentity Current { get; }
}

public class IdentityProvider : IIdentityProvider
{
    public IIdentity Current => new User();
}