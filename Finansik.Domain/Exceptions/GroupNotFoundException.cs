namespace Finansik.Domain.Exceptions;

public class GroupNotFoundException : Exception
{
    public GroupNotFoundException(Guid groupId) : base($"Group with id='{groupId.ToString()}' was not found") { }
}