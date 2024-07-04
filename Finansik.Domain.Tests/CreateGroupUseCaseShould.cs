using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
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
    private readonly ISetup<ICreateGroupStorage, Task<Models.Group>> _createGroupSetup;
    private readonly ISetup<IIntentionManager,bool> _isAllowedSetup;
    private readonly ISetup<IIdentity,Guid> _getUserIdSetup;

    public CreateGroupUseCaseShould()
    {
        var guidFactory = new Mock<IGuidFactory>();
        
        var dbContextOptionsBuilder = new DbContextOptionsBuilder(new DbContextOptions<FinansikDbContext>())
            .UseInMemoryDatabase(nameof(CreateGroupUseCaseShould));
        _dbContext = new FinansikDbContext(dbContextOptionsBuilder.Options);

        var createGroupStorage = new Mock<ICreateGroupStorage>();
        _createGroupSetup = createGroupStorage.Setup(s => s.CreateGroup(
            It.IsAny<string>(), 
            guidFactory.Object.Create(), 
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        
        var intentionManager = new Mock<IIntentionManager>();
        _isAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<GroupIntention>()));
        
        var identity = new Mock<IIdentity>();
        _getUserIdSetup = identity.Setup(i => i.UserId);
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        
        _sut = new CreateGroupUseCase(createGroupStorage.Object, intentionManager.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenCreatingGroupIsNotAllowed()
    {
        _isAllowedSetup.Returns(false);

        await _sut.Invoking(sut => sut.Execute("Some group", "noIcon.png", CancellationToken.None))
            .Should()
            .ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task ReturnsGroupObject()
    {
        var groupName = "new group";
        var groupIcon = "nopic.png";
        var groupId = Guid.Parse("3EE7CC2A-18C3-41C4-84D4-3B1B7ED9EE28");

        _isAllowedSetup.Returns(true);
        _createGroupSetup.ReturnsAsync(new Models.Group
        {
            Id = groupId,
            Icon = groupIcon,
            Name = groupName
        });
            
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

        _isAllowedSetup.Returns(true);
        
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