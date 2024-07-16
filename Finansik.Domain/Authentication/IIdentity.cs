namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
}

public class User(Guid userId) : IIdentity
{
    public Guid UserId { get; set; } = userId;

    public static User Guest => new User(Guid.Empty);
}

internal static class IdentityExtensions
{
    public static bool IsAuthenticated(this IIdentity identity) => identity.UserId != Guid.Empty;
}