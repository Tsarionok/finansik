using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;

namespace Finansik.Storage.Entities;

public sealed class Period : PrimaryKeyEntity<Guid>
{
    public Guid GroupId { get; set; }
    
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }
    
    [InverseProperty(nameof(PeriodCategory.Period))]
    public ICollection<PeriodCategory> PeriodCategories { get; set; }
    
    public DateTimeOffset Start { get; set; }
    
    public DateTimeOffset End { get; set; }
}