namespace Finansik.Domain.Models;

public sealed class RecognisedUser
{
    public Guid UserId { get; set; }
    
    public byte[] Salt { get; set; }
    
    public byte[] PasswordHash { get; set; }
}