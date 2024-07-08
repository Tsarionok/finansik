using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetCategories;
using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests;

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

        await _sut.Invoking(sut => sut.Execute(It.IsAny<Guid>(), CancellationToken.None))
            .Should()
            .ThrowAsync<GroupNotFoundException>();
    }
}