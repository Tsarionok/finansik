using Finansik.Domain.Exceptions;
using Finansik.Domain.Identity;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests;

public class CreateCategoryUseCaseShould
{
    private readonly CreateCategoryUseCase _sut;
    private readonly Mock<ICreateCategoryStorage> _storage;
    private readonly ISetup<ICreateCategoryStorage,Task<bool>> _isGroupExistsSetup;
    private readonly ISetup<ICreateCategoryStorage,Task<Category>> _createCategorySetup;
    private readonly ISetup<IIdentity,Guid> _getCurrentUserIdSetup;

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

        _sut = new CreateCategoryUseCase(_storage.Object, identityProvider.Object);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenNoMatchingGroup()
    {
        _isGroupExistsSetup.ReturnsAsync(false);
        
        const string name = "TestCategory";
        const string icon = "testCategory.png";
        var groupId = Guid.Parse("3B5C4772-7086-41AB-BF78-4B370064B05A");

        await _sut.Invoking(sut => sut.Execute(name, groupId, icon, CancellationToken.None))
            .Should()
            .ThrowAsync<GroupNotFoundException>();
        
        _storage.Verify(s => s.IsGroupExists(groupId, It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task ReturnCreatedCategory_WhenMatchingGroupExists()
    {
        var groupId = Guid.Parse("3B1FCBBA-FE88-491A-843A-65E9716BD7FB");
        var userId = Guid.Parse("0B31EAFE-363F-4CA7-AB65-F453367F8444");
        const string categoryName = "New category";
        const string categoryIcon = "categoryIcon.png";
        
        _isGroupExistsSetup.ReturnsAsync(true);
        _getCurrentUserIdSetup.Returns(userId);
        var expected = new Category();
        _createCategorySetup.ReturnsAsync(expected);
        
        var actual = await _sut.Execute(categoryName, groupId, categoryIcon, CancellationToken.None);
        actual.Should().Be(expected);
        
        _storage.Verify(s => s.CreateCategory(categoryName, groupId, userId, categoryIcon, It.IsAny<CancellationToken>()), Times.Once);
    }
}