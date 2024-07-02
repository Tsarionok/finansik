namespace Finansik.Domain.Identity;

public interface IIdentityProvider
{
    public IIdentity Current { get; }
}