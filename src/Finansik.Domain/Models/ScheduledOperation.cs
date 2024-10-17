namespace Finansik.Domain.Models;

public sealed class ScheduledOperation
{
    public Guid? PeriodCategoryId { get; set; }
    
    // TODO: map to OperationDirection
    public string Direction { get; set; }
    
    public DateTimeOffset? ScheduledFor { get; set; }
    
    public string Description { get; set; }
    
    public decimal Amount { get; set; }
    
    public required string CurrencyCode { get; set; }
}