namespace Finansik.Domain.Models;

public sealed class Group
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public string? Icon { get; set; }
}