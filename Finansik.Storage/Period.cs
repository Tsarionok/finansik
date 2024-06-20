using System.ComponentModel.DataAnnotations.Schema;

namespace Finansik.Storage;

public class Period : KeyedEntity
{
    public Guid GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }
    
    [InverseProperty(nameof(PeriodCategory.Period))]
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public DateTimeOffset Start { get; set; }
    
    public DateTimeOffset End { get; set; }
}