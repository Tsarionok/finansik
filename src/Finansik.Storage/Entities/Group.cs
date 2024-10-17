using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;

namespace Finansik.Storage.Entities;

public sealed class Group : PrimaryKeyEntity<Guid>
{
    public ICollection<User> Users { get; set; }
    
    [InverseProperty(nameof(Period.Group))]
    public ICollection<Period> Periods { get; set; }
    
    public string Name { get; set; }
    
    public string? Icon { get; set; }
    
    public Guid Creator { get; set; }
}