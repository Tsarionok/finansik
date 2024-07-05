namespace Finansik.Domain.Exceptions;

public class CategoryNotFoundException(Guid categoryId) : DomainException(ErrorCodes.Gone, $"Category with id={categoryId} was not found");