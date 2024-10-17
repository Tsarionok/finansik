namespace Finansik.Domain.Authentication;

public interface IIdentity
{
    Guid UserId { get; }
    
    Guid SessionId { get; }

    bool IsAuthenticated();
}