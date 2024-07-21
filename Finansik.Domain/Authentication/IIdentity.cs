namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
    
    Guid SessionId { get; }

    bool IsAuthenticated();
}

public class User(Guid userId, Guid sessionId) : IIdentity
{
    public Guid UserId { get; } = userId;

    public Guid SessionId { get; } = sessionId;

    public static User Guest => new (Guid.Empty, Guid.Empty);
    
    public bool IsAuthenticated() => UserId != Guid.Empty;
}
