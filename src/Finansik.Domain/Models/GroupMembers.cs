namespace Finansik.Domain.Models;

public sealed class GroupMembers
{
    public Guid GroupId { get; set; }
    
    public IEnumerable<GroupUser> Members { get; set; }

    public class GroupUser
    {
        public Guid UserId { get; set; }

        public string Login { get; set; }
    }
}