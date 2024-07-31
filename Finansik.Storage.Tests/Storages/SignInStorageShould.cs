using System.Security.Cryptography;
using Finansik.Storage.Entities;
using Finansik.Storage.Storages;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Finansik.Storage.Tests.Storages;

public class SignInStorageShould : IClassFixture<StorageTestFixture>
{
    private readonly SignInStorage _sut;
    private readonly FinansikDbContext _dbContext;

    public SignInStorageShould(StorageTestFixture fixture)
    {
        _dbContext = fixture.GetDbContext();
        _sut = new SignInStorage(fixture.GetDbContext(), new GuidFactory(), fixture.GetMapper());
    }

    [Fact]
    public async Task ReturnsNotEmptySessionId_WhenSessionIsAddedToDatabaseSuccessfully()
    {
        var userId = Guid.Parse("0A5D3CFE-6298-4E6D-8493-40C41512CB8B");
        await _dbContext.Users.AddAsync(new User
        {
            Id = userId,
            Login = "tester",
            Salt = RandomNumberGenerator.GetBytes(32),
            PasswordHash = RandomNumberGenerator.GetBytes(32)
        }, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        
        var actual = await _sut.CreateSession(
            userId, 
            DateTimeOffset.UtcNow.AddDays(1), 
            CancellationToken.None);

        var sessionsCount = await _dbContext.Sessions.AnyAsync(CancellationToken.None);

        sessionsCount.Should().BeTrue();
        actual.Should().NotBeEmpty();
    }
}