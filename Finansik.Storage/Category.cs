using System.ComponentModel.DataAnnotations.Schema;

namespace Finansik.Storage;

public class Category : KeyedEntity
{
    [InverseProperty(nameof(PeriodCategory.Category))]
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public Guid GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }
    
    public required string Name { get; set; }
}