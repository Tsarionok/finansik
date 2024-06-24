using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateGroup;
using Finansik.Storage;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using Group = Finansik.Storage.Entities.Group;

namespace Finansik.Domain.Tests;

public class CreateGroupUseCaseShould
{
    private readonly ICreateGroupUseCase _sut;
    private readonly FinansikDbContext _dbContext;
    private readonly ISetup<IGuidFactory, Guid>? _setup;

    public CreateGroupUseCaseShould()
    {
        var guidFactory = new Mock<IGuidFactory>();
        
        var dbContextOptionsBuilder = new DbContextOptionsBuilder(new DbContextOptions<FinansikDbContext>())
            .UseInMemoryDatabase(nameof(CreateGroupUseCaseShould));
        _dbContext = new FinansikDbContext(dbContextOptionsBuilder.Options);
        _setup = guidFactory.Setup(g => g.Create());
        _sut = new CreateGroupUseCase(_dbContext, guidFactory.Object);
    }

    [Fact]
    public async Task ReturnsGroupObject()
    {
        var groupName = "new group";
        var groupIcon = "nopic.png";
        var groupId = Guid.Parse("3EE7CC2A-18C3-41C4-84D4-3B1B7ED9EE28");

        _setup?.Returns(groupId);
        var group = await _sut.Execute(groupName, groupIcon, CancellationToken.None);

        group.Should().BeEquivalentTo(new Finansik.Domain.Models.Group
        {
            Id = groupId,
            Name = groupName,
            Icon = groupIcon
        }, args => args
            .Including(g => g.Id)
            .Including(g => g.Name)
            .Including(g => g.Icon));
    }

    [Fact]
    public async Task SaveGroupObjectToDbContext()
    {
        var groupName = "PersonalTest";
        var groupIcon = "person.png";
        var groupId = Guid.Parse("2B40A283-6ACD-4907-9B57-71B12714DA40");
        
        _setup?.Returns(groupId);
        await _sut.Execute(groupName, groupIcon, CancellationToken.None);
        
        var group = await _dbContext.Groups.FirstAsync(g => g.Id == groupId);
        
        group.Should().BeEquivalentTo(new Group 
        {
            Id = groupId,
            Icon = groupIcon,
            Name = groupName
        });
    }
}