namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; set; }
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}