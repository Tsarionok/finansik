namespace Finansik.Domain.Authentication.Cryptography;

public interface ISymmetricDecryptor
{
    Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
}