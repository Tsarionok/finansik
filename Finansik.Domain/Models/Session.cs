namespace Finansik.Domain.Models;

public class Session
{
    public Guid UserId { get; set; }
    
    public DateTimeOffset ExpiresAt { get; set; }
}