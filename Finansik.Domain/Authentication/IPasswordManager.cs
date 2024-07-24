namespace Finansik.Domain.Authentication;

internal interface IPasswordManager
{
    void ThrowIfPasswordNotMatched(string password, byte[] salt, byte[] passwordHash);

    (byte[] salt, byte[] hash) GeneratePasswordParts(string password);
}