namespace Finansik.Domain.Authentication;

internal sealed class IdentityProvider : IIdentityProvider
{
    public IIdentity Current { get; set; } = User.Guest;
}