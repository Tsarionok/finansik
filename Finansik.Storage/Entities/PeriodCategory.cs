using System.ComponentModel.DataAnnotations.Schema;

namespace Finansik.Storage.Entities;

public class PeriodCategory : KeyedEntity
{
    public Guid CategoryId { get; set; }
    
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; }
    
    public Guid PeriodId { get; set; }
    
    [ForeignKey(nameof(PeriodId))]
    public Period Period { get; set; }
    
    [InverseProperty(nameof(ScheduledOperation.PeriodCategory))]
    public ICollection<ScheduledOperation> ScheduledOperations { get; set; }
    
    [InverseProperty(nameof(PerformedOperation.PeriodCategory))]
    public ICollection<PerformedOperation> PerformedOperations { get; set; }
    
    public decimal? AmountLimit { get; set; }
    
    public required string CurrencyCode { get; set; }
}