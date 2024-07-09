using Finansik.Domain.Exceptions.ErrorCodes;

namespace Finansik.Domain.Exceptions;

public class GroupNotFoundException(Guid groupId) : DomainException(DomainErrorCodes.Gone, $"Group with id='{groupId.ToString()}' was not found");