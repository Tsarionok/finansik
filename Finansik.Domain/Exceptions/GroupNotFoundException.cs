namespace Finansik.Domain.Exceptions;

public class GroupNotFoundException(Guid groupId) : Exception($"Group with id='{groupId.ToString()}' was not found");