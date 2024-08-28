using Finansik.Domain.Exceptions.ErrorCodes;

namespace Finansik.Domain.Exceptions;

public class CategoryNotFoundException(Guid categoryId) : 
    DomainException(DomainErrorCodes.Gone, $"Category with id={categoryId} was not found");