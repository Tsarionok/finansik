namespace Finansik.Domain.Exceptions;

public class LoginAlreadyUsedException(string login) : Exception($"User with login={login} already registered");