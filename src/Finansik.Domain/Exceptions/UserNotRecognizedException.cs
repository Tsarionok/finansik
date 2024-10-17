namespace Finansik.Domain.Exceptions;

public class UserNotRecognizedException(string login) : Exception($"User with login=\"{login}\" not recognized");