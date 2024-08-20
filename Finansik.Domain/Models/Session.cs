namespace Finansik.Domain.Models;

public sealed class Session
{
    public Guid UserId { get; set; }
    
    public DateTimeOffset ExpiresAt { get; set; }
}