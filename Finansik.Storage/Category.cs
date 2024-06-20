namespace Finansik.Storage;

public class Category : KeyedEntity
{
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public Guid GroupId { get; set; }
    
    public required string Name { get; set; }
}