using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;

namespace Finansik.Storage.Entities;

public class Session : PrimaryKeyEntity<Guid>
{
    public required Guid UserId { get; set; }
    
    public DateTimeOffset ExpireAt { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }
}