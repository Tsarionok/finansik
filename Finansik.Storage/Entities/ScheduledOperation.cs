using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;
using Finansik.Storage.Entities.Enums;

namespace Finansik.Storage.Entities;

public class ScheduledOperation : PrimaryKeyEntity<Guid>
{
    public Guid? PeriodCategoryId { get; set; }
    
    [ForeignKey(nameof(PeriodCategoryId))]
    public PeriodCategory PeriodCategory { get; set; }
    
    public OperationDirection Direction { get; set; }
    
    public DateTimeOffset? ScheduledFor { get; set; }
    
    public string Description { get; set; }
    
    public decimal Amount { get; set; }
    
    public required string CurrencyCode { get; set; }
}