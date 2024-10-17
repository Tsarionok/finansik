namespace Finansik.Domain.Authentication.Cryptography;

public interface ISymmetricEncryptor
{
    Task<string> Encrypt(string text, byte[] key, CancellationToken cancellationToken);
}