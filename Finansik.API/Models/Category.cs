namespace Finansik.API.Models;

public class Category
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public class CreateCategory
{
    public string Name { get; set; }
    
    public string? Icon { get; set; }
}

public class DeleteCategory
{
    public Guid CategoryId { get; set; }
}