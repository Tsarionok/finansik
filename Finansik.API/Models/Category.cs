namespace Finansik.API.Models;

public sealed class Category
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public sealed class CreateCategory
{
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public sealed class DeleteCategory
{
    public Guid CategoryId { get; set; }
}