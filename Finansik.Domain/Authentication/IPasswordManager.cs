namespace Finansik.Domain.Authentication;

internal interface IPasswordManager
{
    bool ComparePasswords(string password, byte[] salt, byte[] passwordHash);

    void ThrowIfPasswordNotMatched(string password, byte[] salt, byte[] passwordHash);

    (byte[] salt, byte[] hash) GeneratePasswordParts(string password);
}