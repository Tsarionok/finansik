namespace Finansik.Storage;

public class Group : KeyedEntity
{
    public ICollection<User> Users { get; set; }
    
    public string Name { get; set; }
    
    public string Icon { get; set; }
}