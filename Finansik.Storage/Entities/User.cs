using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage.Entities;

public class User : KeyedEntity
{
    public ICollection<Group> Groups { get; set; }
    
    [MaxLength(32)]
    public required string Login { get; set; }
}