using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.GetGroupMembers;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.GetGroupMembers;

public class GetGroupMembersUseCaseShould
{
    private readonly GetGroupMembersUseCase _sut;
    private readonly ISetup<IGetGroupMembersStorage,Task<bool>> _isGroupExistsSetup;
    private readonly ISetup<IGetGroupMembersStorage,Task<GroupMembers>> _findUsersSetup;

    public GetGroupMembersUseCaseShould()
    {
        var storage = new Mock<IGetGroupMembersStorage>();

        _isGroupExistsSetup = storage.Setup(s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        _findUsersSetup = storage.Setup(s => s.FindUsersByGroupId(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        _sut = new GetGroupMembersUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenGroupIdNotFound()
    {
        _isGroupExistsSetup.ReturnsAsync(false);

        var actual =_sut.Invoking(async sut => await sut.Handle(
            new GetGroupMembersQuery(Guid.Parse("295818A8-56FD-4728-8509-70BAB0EEF3EA")), 
            CancellationToken.None));

        await actual.Should().ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnNotEmptyUserList_WhenGroupContainsUsers()
    {
        var groupId = Guid.Parse("14764F1E-9C9E-4D92-85A4-B791D6913B68");
        var user = new GroupMembers
        {
            GroupId = groupId,
            Members = new GroupMembers.GroupUser[]
            {
                new()
                {
                    UserId = Guid.Parse("021B4C29-DA96-48C0-9134-E0F322FF0808"),
                    Login = "ADMIN"
                },
                new()
                {
                    UserId = Guid.Parse("EE3EE88A-4B19-479A-8CDD-90ECE015ADF1"),
                    Login = "Finansist"
                }
            }
        };
        _isGroupExistsSetup.ReturnsAsync(true);
        _findUsersSetup.ReturnsAsync(user);

        var actual = _sut.Handle(new GetGroupMembersQuery(groupId), CancellationToken.None);
        actual.Should().NotBeEquivalentTo(user);
    }
}