using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Storage;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using Group = Finansik.Storage.Entities.Group;

namespace Finansik.Domain.Tests;

public class CreateCategoryUseCaseShould
{
    private readonly CreateCategoryUseCase _sut;
    private readonly FinansikDbContext _dbContext;
    private readonly ISetup<IGuidFactory,Guid>? _guidFactorySetup;

    public CreateCategoryUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<FinansikDbContext>()
            .UseInMemoryDatabase(nameof(CreateCategoryUseCaseShould));
        var guidFactory = new Mock<IGuidFactory>();
        
        _guidFactorySetup = guidFactory.Setup(f => f.Create());
        _dbContext = new FinansikDbContext(dbContextOptionsBuilder.Options);
        _sut = new CreateCategoryUseCase(guidFactory.Object, _dbContext);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenNoMatchingGroup()
    {
        await _dbContext.Groups.AddAsync(new Group
        {
            Id = Guid.Parse("C8DAEB56-B92F-45C7-B311-F3095A3F2ADD"),
            Name = "TestCategory",
            Icon = "TestIcon"
        });
        await _dbContext.SaveChangesAsync();
        
        var name = "TestCategory";
        var icon = "testCategory.png";
        var groupId = Guid.Parse("3B5C4772-7086-41AB-BF78-4B370064B05A");

        await _sut.Invoking(sut => sut.Execute(name, groupId, icon, CancellationToken.None))
            .Should()
            .ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnCreatedCategory()
    {
        var groupId = Guid.Parse("3B1FCBBA-FE88-491A-843A-65E9716BD7FB");
        var categoryName = "New category";
        var categoryIcon = "categoryIcon.png";
        var categoryId = Guid.Parse("FEE76061-340C-4B7D-9754-C228D0CB8E91");
        
        _guidFactorySetup?.Returns(categoryId);
        
        await _dbContext.Groups.AddAsync(new Group
        {
            Id = groupId,
            Name = "Adding group name",
            Icon = "Existing icon"
        });
        await _dbContext.SaveChangesAsync();

        var actual = await _sut.Execute(categoryName, groupId, categoryIcon, CancellationToken.None);

        actual.Should().BeEquivalentTo(new Category
        {
            Id = categoryId,
            Name = categoryName,
            Icon = categoryIcon,
            GroupId = groupId
        }, cfg => cfg
            .Including(c => c.Id)
            .Including(c => c.Name)
            .Including(c => c.Icon)
            .Including(c => c.GroupId));
    }
}