using System.ComponentModel.DataAnnotations.Schema;

namespace Finansik.Storage.Entities;

public class Category : IdentifyingEntity
{
    [InverseProperty(nameof(PeriodCategory.Category))]
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public Guid GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }
    
    public required string Name { get; set; }
    
    public string? Icon { get; set; }
    
    public Guid Creator { get; set; }
}