using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Finansik.Storage.Entities.Abstractions;

namespace Finansik.Storage.Entities;

public sealed class User : PrimaryKeyEntity<Guid>
{
    public ICollection<Group>? Groups { get; set; }
    
    [MaxLength(32)]
    public required string Login { get; set; }
    
    [MaxLength(120)]
    public required byte[] Salt { get; set; }
    
    [MaxLength(300)]
    public required byte[] PasswordHash { get; set; }
    
    [InverseProperty(nameof(Session.User))]
    public ICollection<Session>? Sessions { get; set; }
}