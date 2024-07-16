using System.Security.Cryptography;
using System.Text;

namespace Finansik.Domain.Authentication;

internal class PasswordManager : IPasswordManager
{
    private const int SaltLenght = 100;
    private readonly Lazy<SHA256> _hashAlgorithm = new(SHA256.Create);
    
    public bool ComparePasswords(string password, byte[] salt, byte[] passwordHash) => 
        ComputeHash(password, salt).SequenceEqual(passwordHash);

    public (byte[] salt, byte[] hash) GeneratePasswordParts(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLenght);
        var hash = ComputeHash(password, salt);
        return (salt, hash.ToArray());
    }

    private ReadOnlySpan<byte> ComputeHash(string plainText, byte[] salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var buffer = new byte[plainTextBytes.Length + salt.Length];
        Array.Copy(plainTextBytes, buffer, plainTextBytes.Length);
        Array.Copy(salt, 0, buffer, plainTextBytes.Length, salt.Length);

        lock (_hashAlgorithm)
        {
            return _hashAlgorithm.Value.ComputeHash(buffer);
        }
    }
}