using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage;

public abstract class KeyedEntity
{
    [Key]
    public Guid Id { get; set; }
}