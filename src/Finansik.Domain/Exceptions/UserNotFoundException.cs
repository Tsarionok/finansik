using Finansik.Domain.Exceptions.ErrorCodes;

namespace Finansik.Domain.Exceptions;

public class UserNotFoundException(Guid userId) : 
    DomainException(DomainErrorCodes.Gone, $"User with id='{userId.ToString()}' was not found");