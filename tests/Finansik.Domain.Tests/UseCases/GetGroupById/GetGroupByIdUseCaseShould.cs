using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupById;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.GetGroupById;

public class GetGroupByIdUseCaseShould
{
    private readonly GetGroupByIdUseCase _sut;
    private readonly ISetup<IGetGroupByIdStorage, Task<Group>?> _findGroupSetup;

    public GetGroupByIdUseCaseShould()
    {
        var storage = new Mock<IGetGroupByIdStorage>();
        _findGroupSetup = storage.Setup(storage => storage.FindGroup(It.IsAny<Guid>(), CancellationToken.None));
        
        _sut = new GetGroupByIdUseCase(storage.Object);
    }

    [Fact]
    public async Task ReturnsNotEmptyGroup_WhenGroupIdExists()
    {
        var groupId = Guid.Parse("35CF3118-1372-43DC-A763-C1DB5770B094");
        _findGroupSetup!.ReturnsAsync(new Group
        {
            Id = groupId,
            Name = "Family"
        });
        
        var actual = await _sut.Handle(new GetGroupByIdQuery(groupId), CancellationToken.None);

        actual.Should().BeEquivalentTo(new Group
        {
            Id = groupId,
            Name = "Family"
        }, options => options
            .Including(model => model.Id)
            .Including(model => model.Name));
    }

    [Fact]
    public async Task ThrowsGroupNotFoundException_WhenGroupIdNotExists()
    {
        _findGroupSetup.Returns(() => Task.Run(() => (Group?)null)!);

        var actual = _sut.Invoking(async sut => await sut.Handle(new GetGroupByIdQuery(
            Guid.Parse("EB6044C4-8A2A-4847-925B-63A88F35CA96")), CancellationToken.None));

        await actual.Should().ThrowAsync<GroupNotFoundException>();
    }
}