using Finansik.Domain.Exceptions;
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

    public GetCategoriesByGroupIdUseCaseShould()
    {
        var storage = new Mock<IGetCategoriesByGroupIdStorage>();
        _isGroupExistsSetup = storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
            
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

    [Fact]
    public async Task ReturnEmptyList_WhenNoAnyCategoryMatchedByGroup()
    {
        var groupId = Guid.Parse("F641B767-FC67-4D0B-92E8-3A4325FA5317");

        var resultCategories = await _sut.Execute(groupId, CancellationToken.None);
        resultCategories.Should().BeEmpty();
    }

    [Fact]
    public async Task ReturnOnlyCategoriesThatBelongToGroup()
    {
        var accountingGroupId = Guid.Parse("C35BA9DD-9546-4370-A0E4-ACB8F485CCF8");

        var accountingCategories = new List<Guid>
        {
            Guid.Parse("5C187A60-F46F-4A82-BE00-19A59A3F881B"),
            Guid.Parse("BF62F610-A4CE-48C9-817C-2092D6E0B05F")
        };

        var resultCategories = await _sut.Execute(accountingGroupId, CancellationToken.None);
        resultCategories.Select(c => c.Id)
            .Should()
            .BeEquivalentTo(accountingCategories);
    }
}