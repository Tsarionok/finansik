namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

public class User : IIdentity
{
    public Guid UserId => Guid.Parse("A12ED44F-4EA9-46A2-86FD-7F8F3900CFD3");
}

public static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}