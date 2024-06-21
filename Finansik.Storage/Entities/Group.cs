using System.ComponentModel.DataAnnotations.Schema;

namespace Finansik.Storage.Entities;

public class Group : KeyedEntity
{
    public ICollection<User> Users { get; set; }
    
    [InverseProperty(nameof(Period.Group))]
    public ICollection<Period> Periods { get; set; }
    
    public string Name { get; set; }
    
    public string Icon { get; set; }
}