namespace Finansik.Domain.Exceptions;

public class IntentionManagerException() : DomainException(ErrorCodes.Forbidden, "Action is not allowed");