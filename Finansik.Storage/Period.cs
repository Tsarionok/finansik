namespace Finansik.Storage;

public class Period : KeyedEntity
{
    public Guid GroupId { get; set; }
    
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public DateTimeOffset Start { get; set; }
    
    public DateTimeOffset End { get; set; }
}