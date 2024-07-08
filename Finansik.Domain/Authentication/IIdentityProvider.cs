namespace Finansik.Domain.Authentication;

public interface IIdentityProvider
{
    public IIdentity Current { get; }
}

public class IdentityProvider : IIdentityProvider
{
    // TODO: remove magic const
    public IIdentity Current => new User(Guid.Parse("A12ED44F-4EA9-46A2-86FD-7F8F3900CFD3"));
}