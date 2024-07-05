namespace Finansik.Domain.Exceptions;

public class GroupNotFoundException(Guid groupId) : DomainException(ErrorCodes.Gone, $"Group with id='{groupId.ToString()}' was not found");