using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Common;

namespace Finansik.Storage;

public class PerformedOperation : KeyedEntity
{
    public Guid? ScheduledOperationId { get; set; }
    
    [ForeignKey(nameof(ScheduledOperationId))]
    public ScheduledOperation? ScheduledOperation { get; set; }
    
    public Guid PeriodCategoryId { get; set; }
    
    [ForeignKey(nameof(PeriodCategoryId))]
    public PeriodCategory PeriodCategory { get; set; }
    
    public decimal Amount { get; set; }
    
    public required string CurrencyCode { get; set; }
    
    public OperationDirection Direction { get; set; }
    
    public string? Description { get; set; }
    
    public DateTimeOffset PerformedAt { get; set; }
}