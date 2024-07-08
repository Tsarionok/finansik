namespace Finansik.Domain.Models;

public class Category
{
    public Guid Id { get; set; }
    
    public required string Name { get; set; }
    
    public Guid GroupId { get; set; }
    
    public string? Icon { get; set; }
}