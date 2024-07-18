using System.Security.Cryptography;
using Finansik.Domain.Authentication.Cryptography;
using FluentAssertions;
using Xunit.Abstractions;

namespace Finansik.Domain.Tests.Authentication;

public class AesSymmetricEncryptorDecryptorShould(ITestOutputHelper testOutputHelper)
{
    private readonly AesSymmetricEncryptorDecryptor _sut = new();

    [Fact]
    public async Task ReturnMeaningfulEncryptedString()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var actual = await _sut.Encrypt("Hello world!", key, CancellationToken.None);

        actual.Should().NotBeEmpty();
    }
    
    [Fact]
    public async Task ReturnEncryptedString_WhenKeyIsSame()
    {
        const string text = "Hello world";
        
        var key = RandomNumberGenerator.GetBytes(32);
        var encrypted = await _sut.Encrypt(text, key, CancellationToken.None);
        var actual = await _sut.Decrypt(encrypted, key, CancellationToken.None);

        actual.Should().Be(text);
    }

    [Fact]
    public async Task ThrowCryptographicException_WhenDecryptingWithDifferentKey()
    {
        var encrypted = await _sut.Encrypt("Hello, world!", RandomNumberGenerator.GetBytes(32), CancellationToken.None);
        await _sut.Invoking(sut => sut.Decrypt(encrypted, RandomNumberGenerator.GetBytes(32), CancellationToken.None))
            .Should().ThrowAsync<CryptographicException>();
    }

    [Fact]
    public void GiveMeBase64Key()
    {
        testOutputHelper.WriteLine(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
    }
}