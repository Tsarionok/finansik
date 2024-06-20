namespace Finansik.Storage;

public class PeriodCategory : KeyedEntity
{
    public Guid CategoryId { get; set; }
    
    public Guid PeriodId { get; set; }
    
    public decimal? AmountLimit { get; set; }
    
    public required string CurrencyCode { get; set; }
}