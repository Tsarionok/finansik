using Finansik.Domain.Exceptions;
using Finansik.Domain.UseCases.RemoveMemberFromGroup;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.RemoveMemberFromGroup;

public class RemoveMemberFromGroupUseCaseShould
{
    private readonly RemoveMemberFromGroupUseCase _sut;
    private readonly ISetup<IRemoveMemberFromGroupStorage,Task<bool>> _isUserExistsSetup;
    private readonly ISetup<IRemoveMemberFromGroupStorage,Task<bool>> _isGroupExistsSetup;

    public RemoveMemberFromGroupUseCaseShould()
    {
        var storage = new Mock<IRemoveMemberFromGroupStorage>();
        _isUserExistsSetup = storage.Setup(
            s => s.IsUserExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        _isGroupExistsSetup = storage.Setup(
            s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        _sut = new RemoveMemberFromGroupUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowUserNotFoundException_WhenUserIdNotFound()
    {
        _isUserExistsSetup.ReturnsAsync(false);
        _isGroupExistsSetup.ReturnsAsync(true);

        var actual = _sut.Invoking(sut => sut.Handle(
            new RemoveMemberFromGroupCommand(
                Guid.Parse("3E267693-246A-45C7-BD8A-C0E4177E058B"),
                Guid.Parse("4D4B1078-1F22-4950-913B-C0DA735FAEFF")), CancellationToken.None));

        await actual.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenGroupIdNotFound()
    {
        _isUserExistsSetup.ReturnsAsync(true);
        _isGroupExistsSetup.ReturnsAsync(false);

        var actual = _sut.Invoking(sut => sut.Handle(
            new RemoveMemberFromGroupCommand(
                Guid.Parse("8093908A-ED85-4524-B918-07C3493E164C"),
                Guid.Parse("1E5E1E75-31F3-438C-8E4C-660351FB130C")), CancellationToken.None));

        await actual.Should().ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnsUserId_WhenUserRemovedFromGroup()
    {
        const string userId = "90A10A0A-BE4B-43A5-9E28-8A31A70D4C63";
        _isUserExistsSetup.ReturnsAsync(true);
        _isGroupExistsSetup.ReturnsAsync(true);
        
        var actual = await _sut.Handle(new RemoveMemberFromGroupCommand(
            Guid.Parse("B10AA4F5-A2C6-44F8-ACF7-B0D1EE1BE3BF"),
            Guid.Parse(userId)), CancellationToken.None);

        actual.Should().Be(userId);
    }
}