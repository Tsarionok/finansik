namespace Finansik.Domain.Authentication;

public interface IDecryptor
{
    Task<string> Decode(string encryptedText, byte[] key, CancellationToken cancellationToken);
}