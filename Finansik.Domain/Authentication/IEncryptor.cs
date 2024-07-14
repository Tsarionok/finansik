namespace Finansik.Domain.Authentication;

public interface IEncryptor
{
    Task<string> Encrypt(string text, byte[] key, CancellationToken cancellationToken);
}