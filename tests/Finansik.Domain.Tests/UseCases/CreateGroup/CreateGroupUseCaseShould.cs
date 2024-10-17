using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Group;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateGroup;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.CreateGroup;

[TestSubject(typeof(CreateGroupUseCase))]
public class CreateGroupUseCaseShould
{
    private readonly CreateGroupUseCase _sut;
    private readonly ISetup<ICreateGroupStorage, Task<Group>> _createGroupSetup;
    private readonly ISetup<IIntentionManager,bool> _isAllowedSetup;

    public CreateGroupUseCaseShould()
    {
        var createGroupStorage = new Mock<ICreateGroupStorage>();
        _createGroupSetup = createGroupStorage.Setup(s => s.CreateGroup(
            It.IsAny<string>(), 
            It.IsAny<Guid>(), 
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()));
        
        var intentionManager = new Mock<IIntentionManager>();
        _isAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<GroupIntention>()));
        
        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        
        _sut = new CreateGroupUseCase(createGroupStorage.Object, intentionManager.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenCreatingGroupIsNotAllowed()
    {
        _isAllowedSetup.Returns(false);

        await _sut.Invoking(sut => sut.Handle(
                new CreateGroupCommand("Some group", "noIcon.png"), CancellationToken.None))
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
        _createGroupSetup.ReturnsAsync(new Group
        {
            Id = groupId,
            Icon = groupIcon,
            Name = groupName
        });
            
        var group = await _sut.Handle(
            new CreateGroupCommand(groupName, groupIcon), CancellationToken.None);

        group.Should().BeEquivalentTo(new Group
        {
            Id = groupId,
            Name = groupName,
            Icon = groupIcon
        }, args => args
            .Including(g => g.Id)
            .Including(g => g.Name)
            .Including(g => g.Icon));
    }
}