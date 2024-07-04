using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.GetCategories;
using Finansik.Storage;
using Finansik.Storage.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests;

public class GetCategoriesByGroupIdUseCaseShould
{
    private readonly IGetCategoriesByGroupIdUseCase _sut;
    private readonly FinansikDbContext _dbContext;
    private readonly ISetup<IGetCategoriesByGroupIdStorage,Task<bool>> _isGroupExistsSetup;

    public GetCategoriesByGroupIdUseCaseShould()
    {
        var dbContextOptionsBuilder =
            new DbContextOptionsBuilder().UseInMemoryDatabase(nameof(GetCategoriesByGroupIdUseCaseShould));

        _dbContext = new FinansikDbContext(dbContextOptionsBuilder.Options);

        var storage = new Mock<IGetCategoriesByGroupIdStorage>();
        _isGroupExistsSetup = storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            
        _sut = new GetCategoriesByGroupIdUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenGroupIdNotMatched()
    {
        _isGroupExistsSetup.ReturnsAsync(false);

        await _sut.Invoking(sut => sut.Execute(It.IsAny<Guid>(), CancellationToken.None))
            .Should()
            .ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnEmptyList_WhenNoAnyCategoryMatchedByGroup()
    {
        var groupId = Guid.Parse("F641B767-FC67-4D0B-92E8-3A4325FA5317");
        var existingGroup = new Group
        {
            Id = groupId,
            Name = "Family",
            Icon = "people.png"
        };

        await _dbContext.Groups.AddAsync(existingGroup, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var resultCategories = await _sut.Execute(groupId, CancellationToken.None);
        resultCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnOnlyCategoriesThatBelongToGroup()
    {
        var accountingGroupId = Guid.Parse("C35BA9DD-9546-4370-A0E4-ACB8F485CCF8");
        var personalGroupId = Guid.Parse("BF6606C1-675F-49B4-B128-4E78E8597E4D");

        var groups = new List<Group>
        {
            new()
            {
                Id = accountingGroupId,
                Name = "Accounting department",
                Icon = "office.ico"
            },
            new()
            {
                Id = personalGroupId,
                Name = "Personal Alex",
                Icon = "alexAva.png"
            }
        };

        var accountingCategories = new List<Guid>
        {
            Guid.Parse("5C187A60-F46F-4A82-BE00-19A59A3F881B"),
            Guid.Parse("BF62F610-A4CE-48C9-817C-2092D6E0B05F")
        };

        var categories = new List<Category>
        {
            new()
            {
                Id = accountingCategories[0],
                Name = "Salaries",
                Icon = "money.ico",
                GroupId = accountingGroupId
            },
            new()
            {
                Id = accountingCategories[1],
                Name = "Office devices",
                Icon = "printer.ico",
                GroupId = accountingGroupId
            },
            new()
            {
                Id = Guid.Parse("C1DED2AB-E5ED-4A44-B519-BB3E31A3BC15"),
                Name = "Relax",
                Icon = "chill.ico",
                GroupId = personalGroupId
            }
        };

        await _dbContext.Groups.AddRangeAsync(groups, CancellationToken.None);
        await _dbContext.Categories.AddRangeAsync(categories, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var resultCategories = await _sut.Execute(accountingGroupId, CancellationToken.None);
        resultCategories.Select(c => c.Id)
            .Should()
            .BeEquivalentTo(accountingCategories);
    }
}