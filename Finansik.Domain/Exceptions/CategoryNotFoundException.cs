namespace Finansik.Domain.Exceptions;

public class CategoryNotFoundException(Guid categoryId) : Exception($"Category with id={categoryId} was not found");