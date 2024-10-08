using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Exceptions.ErrorCodes;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.CreateCategory;

[TestSubject(typeof(CreateCategoryUseCase))]
public class CreateCategoryUseCaseShould
{
    private readonly CreateCategoryUseCase _sut;
    private readonly Mock<ICreateCategoryStorage> _storage;
    private readonly ISetup<ICreateCategoryStorage,Task<bool>> _isGroupExistsSetup;
    private readonly ISetup<ICreateCategoryStorage,Task<Category>> _createCategorySetup;
    private readonly ISetup<IIdentity,Guid> _getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager,bool> _intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> _intentionManager;

    public CreateCategoryUseCaseShould()
    {
        _storage = new Mock<ICreateCategoryStorage>();
        _isGroupExistsSetup = _storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _createCategorySetup = _storage.Setup(s =>
            s.CreateCategory(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        _getCurrentUserIdSetup = identity.Setup(i => i.UserId);
        
        _intentionManager = new Mock<IIntentionManager>();
        _intentionIsAllowedSetup = _intentionManager.Setup(m => m.IsAllowed(It.IsAny<CategoryIntention>()));

        _sut = new CreateCategoryUseCase(_storage.Object, identityProvider.Object, _intentionManager.Object);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenNoMatchingGroup()
    {
        _isGroupExistsSetup.ReturnsAsync(false);
        _intentionIsAllowedSetup.Returns(true);
        
        const string name = "TestCategory";
        const string icon = "testCategory.png";
        var groupId = Guid.Parse("3B5C4772-7086-41AB-BF78-4B370064B05A");
        var command = new CreateCategoryCommand(groupId, name, icon);

        var actual = await _sut.Invoking(sut => sut.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<GroupNotFoundException>();
        
        actual.Which.ErrorCode.Should().Be(DomainErrorCodes.Gone);
        actual.Which.Message.Should().NotBeEmpty();
        
        _storage.Verify(s => s.IsGroupExists(groupId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenCreatingCategoryIsNotAllowed()
    {
        var groupId = Guid.Parse("69D3DA00-1C40-4FAC-9342-42CA8308674B");
        
        _intentionIsAllowedSetup.Returns(false);

        await _sut.Invoking(sut => sut.Handle(new CreateCategoryCommand(groupId, "Some category", null), CancellationToken.None))
            .Should()
            .ThrowAsync<IntentionManagerException>();
        
        _intentionManager.Verify(m => m.IsAllowed(CategoryIntention.Create));
    }

    [Fact]
    public async Task ReturnCreatedCategory_WhenMatchingGroupExists()
    {
        var groupId = Guid.Parse("3B1FCBBA-FE88-491A-843A-65E9716BD7FB");
        var userId = Guid.Parse("0B31EAFE-363F-4CA7-AB65-F453367F8444");
        const string categoryName = "New category";
        const string categoryIcon = "categoryIcon.png";
        
        _intentionIsAllowedSetup.Returns(true);
        _isGroupExistsSetup.ReturnsAsync(true);
        _getCurrentUserIdSetup.Returns(userId);
        var expected = new Category
        {
            Name = "Hello world"
        };
        _createCategorySetup.ReturnsAsync(expected);
        
        var actual = await _sut.Handle(new CreateCategoryCommand(groupId, categoryName , categoryIcon), CancellationToken.None);
        actual.Should().Be(expected);
        
        _storage.Verify(s => s.CreateCategory(categoryName, groupId, userId, categoryIcon, It.IsAny<CancellationToken>()), Times.Once);
    }
}