using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage.Entities;

public abstract class IdentifyingEntity
{
    [Key]
    public Guid Id { get; set; }
}