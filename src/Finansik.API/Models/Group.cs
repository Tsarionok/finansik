namespace Finansik.API.Models;

public sealed class Group
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public sealed record CreateGroup(string Name, string? Icon);

public sealed record AddMember(Guid UserId);