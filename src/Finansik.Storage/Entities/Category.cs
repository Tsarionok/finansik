using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;

namespace Finansik.Storage.Entities;

public sealed class Category : PrimaryKeyEntity<Guid>
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