namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }

    bool IsAuthenticated();
}

public class User(Guid userId) : IIdentity
{
    public Guid UserId { get; } = userId;

    public static User Guest => new (Guid.Empty);
    
    public bool IsAuthenticated() => UserId != Guid.Empty;
}
