using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage.Entities;

public abstract class KeyedEntity
{
    [Key]
    public Guid Id { get; set; }
}