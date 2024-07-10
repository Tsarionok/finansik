using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetCategories;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.GetCategoriesByGroupId;

[TestSubject(typeof(GetCategoriesByGroupIdUseCase))]
public class GetCategoriesByGroupIdUseCaseShould
{
    private readonly IGetCategoriesByGroupIdUseCase _sut;
    private readonly ISetup<IGetCategoriesByGroupIdStorage,Task<bool>> _isGroupExistsSetup;
    private readonly ISetup<IGetCategoriesByGroupIdStorage,Task<IEnumerable<Category>>> _getCategoriesByGroupIdSetup;

    public GetCategoriesByGroupIdUseCaseShould()
    {
        var storage = new Mock<IGetCategoriesByGroupIdStorage>();
        _isGroupExistsSetup = storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _getCategoriesByGroupIdSetup = storage.Setup(s => s.GetCategoriesByGroupId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            
        _sut = new GetCategoriesByGroupIdUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenGroupIdNotMatched()
    {
        _isGroupExistsSetup.ReturnsAsync(false);

        await _sut.Invoking(sut => sut.ExecuteAsync(new GetCategoriesByGroupIdCommand(It.IsAny<Guid>()), CancellationToken.None))
            .Should()
            .ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnsCategoryList()
    {
        var categories = new Category[]
        {
            new()
            {
                Id = Guid.Parse("3E1E048F-28D4-4F84-915A-7A1D1A225AAC"),
                Name = "Transport"
            },
            new()
            {
                Id = Guid.Parse("8F712CB7-3A1B-489D-950E-EBA23927E582"),
                Name = "Shopping"
            }
        };
        
        _isGroupExistsSetup.ReturnsAsync(true);
        _getCategoriesByGroupIdSetup.ReturnsAsync(categories);

        var actual = await _sut.ExecuteAsync(
                new GetCategoriesByGroupIdCommand(Guid.Parse("4C0545DF-4096-4420-8237-733B0A2490ED")),
                CancellationToken.None);

        actual.Should().BeEquivalentTo(categories, a => a
            .Including(c => c.Id)
            .Including(c => c.Name));
    }
}