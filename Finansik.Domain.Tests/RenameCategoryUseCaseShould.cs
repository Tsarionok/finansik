using Finansik.Domain.Authentication;
using Finansik.Domain.Authorization;
using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.CreateCategory;
using Finansik.Domain.UseCases.RenameCategory;
using Finansik.Storage;
using Finansik.Storage.Entities;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests;

[TestSubject(typeof(RenameCategoryUseCase))]
public class RenameCategoryUseCaseShould
{
    private readonly IRenameCategoryUseCase _sut;
    private readonly FinansikDbContext _dbContext;
    private readonly ISetup<IRenameCategoryStorage,Task<bool>> _isCategoryExistsSetup;
    private readonly ISetup<IIdentity,Guid> _getCurrentSetup;
    private readonly ISetup<IIntentionManager,bool> _isAllowedSetup;

    public RenameCategoryUseCaseShould()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder()
            .UseInMemoryDatabase(nameof(RenameCategoryUseCaseShould));
            
        _dbContext = new FinansikDbContext(dbContextOptionsBuilder.Options);

        var storage = new Mock<IRenameCategoryStorage>();
        _isCategoryExistsSetup = storage.Setup(s => s.IsCategoryExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        var identity = new Mock<IIdentity>();
        _getCurrentSetup = identity.Setup(i => i.UserId);
        
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);

        var intentionManager = new Mock<IIntentionManager>();
        _isAllowedSetup = intentionManager.Setup(m => m.IsAllowed(CategoryIntention.Rename));
        
        _sut = new RenameCategoryUseCase(storage.Object, identityProvider.Object, intentionManager.Object);
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenCategoryIdNotExist()
    {
        var notExistingCategoryId = Guid.Parse("19CB7652-61CE-4A53-8AAD-4C298350A150");
        var categoryName = "Renamed category";

        await _sut.Invoking(sut => sut.Execute(notExistingCategoryId, categoryName, CancellationToken.None))
            .Should()
            .ThrowAsync<CategoryNotFoundException>();
    }

    [Fact]
    public async Task ReturnsCategoryObjectWithUpdatedName()
    {
        var currentCategoryName = "Personal";
        var updatedCategoryName = "My personal category";
        var currentCategoryId = Guid.Parse("54F1C0A8-E535-47F5-B66C-0013AA349558");
        
        var currentCategory = new Category
        {
            Id = currentCategoryId,
            Name = currentCategoryName
        };

        await _dbContext.Categories.AddAsync(currentCategory);
        await _dbContext.SaveChangesAsync();

        var updatedCategory = await _sut.Execute(currentCategoryId, updatedCategoryName, CancellationToken.None);
        updatedCategory.Should().BeEquivalentTo(new Category
        {
            Id = currentCategoryId,
            Name = updatedCategoryName
        }, opt => opt
            .Including(c => c.Id)
            .Including(c => c.Name));
    }

    [Fact]
    public async Task SaveUpdatedCategoryName()
    {
        var categoryId = Guid.Parse("EB847E30-E3BD-43E9-9B75-F39AB2231B59");
        var currentCategoryName = "Family";
        var updatedCategoryName = "Common";
        var category = new Category
        {
            Id = categoryId,
            Name = currentCategoryName
        };

        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();

        await _sut.Execute(categoryId, updatedCategoryName, CancellationToken.None);

        var updatedCategory = await _dbContext.Categories.FirstAsync(c => c.Id == categoryId, CancellationToken.None);
        updatedCategory.Name.Should().BeEquivalentTo(updatedCategoryName);
    }
}