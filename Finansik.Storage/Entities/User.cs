using System.ComponentModel.DataAnnotations;

namespace Finansik.Storage.Entities;

public class User : IdentifyingEntity
{
    public ICollection<Group> Groups { get; set; }
    
    [MaxLength(32)]
    public required string Login { get; set; }
    
    [MaxLength(120)]
    public string Salt { get; set; }
    
    [MaxLength(300)]
    public string PasswordHash { get; set; }
}