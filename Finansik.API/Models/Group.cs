namespace Finansik.API.Models;

public class Group
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public record CreateGroup(string Name, string? Icon);