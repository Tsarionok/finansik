using System.Security.Cryptography;
using Finansik.Storage.Entities;
using Finansik.Storage.Storages;
using FluentAssertions;

namespace Finansik.Storage.Tests.Storages;

public class CreateCategoryStorageShould : IClassFixture<StorageTestFixture>
{
    private readonly CreateCategoryStorage _sut;
    private readonly FinansikDbContext _dbContext;

    public CreateCategoryStorageShould(StorageTestFixture fixture)
    {
        _sut = new CreateCategoryStorage(fixture.GetDbContext(), new GuidFactory(), fixture.GetMapper());
        _dbContext = fixture.GetDbContext();
    }
    
    [Fact]
    public async Task InsertNewCategoryInDatabase()
    {
        var groupId = Guid.Parse("4E8B58A2-89DB-4775-9D1A-4886E6812A24");
        var userId = Guid.Parse("8F994B1D-A586-473D-BD98-5A9B61635F6A");
        await _dbContext.Groups.AddAsync(new Group { Id = groupId, Name = "Testers"});
        await _dbContext.Users.AddAsync(new User
        {
            Id = userId, 
            Login = "admin", 
            PasswordHash = RandomNumberGenerator.GetBytes(32),
            Salt = RandomNumberGenerator.GetBytes(128)
        });
        await _dbContext.SaveChangesAsync();
        
        var category = await _sut.CreateCategory(
            "Transport",
            groupId,
            userId,
            "icon.png",
            CancellationToken.None);

        category.Id.Should().NotBeEmpty();
    }
}