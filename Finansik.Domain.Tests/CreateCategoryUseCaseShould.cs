using Finansik.Domain.Exceptions;
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

    public CreateCategoryUseCaseShould()
    {
        _storage = new Mock<ICreateCategoryStorage>();
        _isGroupExistsSetup = _storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _createCategorySetup = _storage.Setup(s =>
            s.CreateCategory(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string?>(), It.IsAny<CancellationToken>()));

        _sut = new CreateCategoryUseCase(_storage.Object);
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
        _isGroupExistsSetup.ReturnsAsync(true);
        var expected = new Category();
        _createCategorySetup.ReturnsAsync(expected);
        
        var groupId = Guid.Parse("3B1FCBBA-FE88-491A-843A-65E9716BD7FB");
        const string categoryName = "New category";
        const string categoryIcon = "categoryIcon.png";

        var actual = await _sut.Execute(categoryName, groupId, categoryIcon, CancellationToken.None);
        actual.Should().Be(expected);
        
        _storage.Verify(s => s.CreateCategory(categoryName, groupId, categoryIcon, It.IsAny<CancellationToken>()), Times.Once);
    }
}