using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupById;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.GetGroupById;

public class GetGroupByIdUseCaseShould
{
    private readonly GetGroupByIdUseCase _sut;
    private readonly ISetup<IGetGroupByIdStorage,Task<Group>> _findGroupSetup;

    public GetGroupByIdUseCaseShould()
    {
        var storage = new Mock<IGetGroupByIdStorage>();
        _findGroupSetup = storage.Setup(storage => storage.FindGroup(It.IsAny<Guid>(), CancellationToken.None));
        
        _sut = new GetGroupByIdUseCase(storage.Object);
    }

    [Fact]
    public async Task ReturnsNotEmptyGroup()
    {
        var groupId = Guid.Parse("35CF3118-1372-43DC-A763-C1DB5770B094");
        _findGroupSetup.ReturnsAsync(new Group
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
}