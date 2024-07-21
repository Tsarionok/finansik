using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage.Entities.Abstractions;

public abstract class PrimaryKeyEntity<T> where T : IComparable<T>
{
    [Key]
    public required T Id { get; init; }
}