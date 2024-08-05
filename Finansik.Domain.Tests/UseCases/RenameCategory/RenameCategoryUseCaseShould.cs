using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Authorization.Category;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.RenameCategory;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.RenameCategory;

[TestSubject(typeof(RenameCategoryUseCase))]
public class RenameCategoryUseCaseShould
{
    private readonly RenameCategoryUseCase _sut;
    private readonly ISetup<IRenameCategoryStorage,Task<bool>> _isCategoryExistsSetup;
    private readonly ISetup<IIntentionManager,bool> _isAllowedSetup;
    private readonly ISetup<IRenameCategoryStorage,Task<Category>> _renameCategorySetup;

    public RenameCategoryUseCaseShould()
    {
        var identity = new Mock<IIdentity>();
        var storage = new Mock<IRenameCategoryStorage>();
        
        _isCategoryExistsSetup = storage.Setup(s => s.IsCategoryExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _renameCategorySetup = storage.Setup(s => s.RenameCategory(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);

        var intentionManager = new Mock<IIntentionManager>();
        _isAllowedSetup = intentionManager.Setup(m => m.IsAllowed(CategoryIntention.Rename));
        
        _sut = new RenameCategoryUseCase(storage.Object, intentionManager.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenActionNotAllow()
    {
        _isAllowedSetup.Returns(false);

        await _sut.Invoking(sut => sut.Handle(It.IsAny<RenameCategoryCommand>(), It.IsAny<CancellationToken>()))
            .Should()
            .ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenCategoryIdNotExist()
    {
        _isAllowedSetup.Returns(true);
        _isCategoryExistsSetup.ReturnsAsync(false);

        await _sut.Invoking(sut => sut.Handle(
                new RenameCategoryCommand(It.IsAny<Guid>(), It.IsAny<string>()), CancellationToken.None))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    [Fact]
    public async Task ReturnsCategoryObjectWithUpdatedName()
    {
        var categoryName = "My personal category";
        var categoryId = Guid.Parse("54F1C0A8-E535-47F5-B66C-0013AA349558");
        var groupId = Guid.Parse("E925DA5F-BD4D-4A40-AFFF-6606B2C3146F");
        var icon = "person.png";
        
        _isAllowedSetup.Returns(true);
        _isCategoryExistsSetup.ReturnsAsync(true);
        
        _renameCategorySetup.ReturnsAsync(new Category
        {
            Id = categoryId,
            Name = categoryName,
            GroupId = groupId,
            Icon = icon
        });
        
        var updatedCategory = await _sut.Handle(
            new RenameCategoryCommand(categoryId, categoryName), CancellationToken.None);
        updatedCategory.Should().BeEquivalentTo(new Category
        {
            Id = categoryId,
            Name = categoryName,
            GroupId = groupId,
            Icon = icon,
        }, opt => opt
            .Including(c => c.Id)
            .Including(c => c.Name)
            .Including(c => c.GroupId)
            .Including(c => c.Icon));
    }
}