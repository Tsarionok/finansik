using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
using Finansik.Domain.Exceptions.ErrorCodes;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.DeleteCategory;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.DeleteCategory;

public class DeleteCategoryUseCaseShould
{
    private readonly IDeleteCategoryUseCase _sut;
    private readonly ISetup<IIntentionManager,bool> _isDeletionAllowedSetup;
    private readonly ISetup<IDeleteCategoryStorage,Task<Category>> _deleteCategorySetup;
    private readonly ISetup<IDeleteCategoryStorage,Task<bool>> _isCategoryExistsSetup;
    private readonly Mock<IDeleteCategoryStorage> _storage;

    public DeleteCategoryUseCaseShould()
    {
        var validator = new Mock<IValidator<DeleteCategoryCommand>>();
        var intentionManager = new Mock<IIntentionManager>();
        _isDeletionAllowedSetup = intentionManager.Setup(m => m.IsAllowed(CategoryIntention.Delete));

        _storage = new Mock<IDeleteCategoryStorage>();
        _isCategoryExistsSetup = _storage.Setup(s => s.IsCategoryExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _deleteCategorySetup = _storage.Setup(s => s.DeleteCategory(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        _sut = new DeleteCategoryUseCase(validator.Object, intentionManager.Object, _storage.Object);
    }

    [Fact]
    public async Task ThrowsIntentionManagerException_WhenDeleteCategoryIsNotAllow()
    {
        _isDeletionAllowedSetup.Returns(false);

        await _sut.Invoking(sut => sut.ExecuteAsync(
                new DeleteCategoryCommand(
                    Guid.Parse("34B67E0E-6A27-4DDC-8F49-70E4F68575A3")), CancellationToken.None))
            .Should()
            .ThrowAsync<IntentionManagerException>();
    }

    [Fact]
    public async Task ThrowsCategoryNotFoundException_WhenCategoryIsNotFound()
    {
        _isDeletionAllowedSetup.Returns(true);
        _isCategoryExistsSetup.ReturnsAsync(false);

        var actual = await _sut.Invoking(sut => sut
                .ExecuteAsync(
                    new DeleteCategoryCommand(Guid.Parse("8ADAE1BD-A5BB-438E-B260-1CB00968D985")),
                    CancellationToken.None))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
            
        actual.Which.ErrorCode.Should().Be(DomainErrorCodes.Gone);
        actual.Which.Message.Should().NotBeEmpty();
    }

    [Fact]
    public async Task ReturnDeletedCategory_WhenCategoryWasDeleted()
    {
        var categoryId = Guid.Parse("AEB6D8F4-2B32-4908-848C-4DF0D0C1537A");
        
        _isDeletionAllowedSetup.Returns(true);
        _isCategoryExistsSetup.ReturnsAsync(true);
        _deleteCategorySetup.ReturnsAsync(new Category
        {
            Id = categoryId,
            Name = "Deleted"
        });

        var actual = await _sut.ExecuteAsync(new DeleteCategoryCommand(categoryId), CancellationToken.None);
        actual.Id.Should().Be(categoryId);
        
        _storage.Verify(s => s.DeleteCategory(categoryId, It.IsAny<CancellationToken>()), Times.Once);
    }
}