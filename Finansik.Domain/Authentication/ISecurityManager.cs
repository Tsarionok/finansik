namespace Finansik.Domain.Authentication;

internal interface ISecurityManager
{
    bool ComparePasswords(string password, string salt, string passwordHash);

    (string salt, string hash) GeneratePasswordParts(string password);
}