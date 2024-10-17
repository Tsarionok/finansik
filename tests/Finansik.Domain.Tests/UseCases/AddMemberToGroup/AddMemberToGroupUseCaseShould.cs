using Finansik.Domain.Exceptions;
using Finansik.Domain.Models;
using Finansik.Domain.UseCases.AddMemberToGroup;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Finansik.Domain.Tests.UseCases.AddMemberToGroup;

public class AddMemberToGroupUseCaseShould
{
    private readonly AddMemberToGroupUseCase _sut;
    private readonly ISetup<IAddMemberToGroupStorage,Task<bool>> _isUserExistsSetup;
    private readonly ISetup<IAddMemberToGroupStorage,Task<GroupMembers>> _getUsersSetup;
    private readonly ISetup<IAddMemberToGroupStorage,Task<bool>> _isGroupExistsSetup;

    public AddMemberToGroupUseCaseShould()
    {
        var storage = new Mock<IAddMemberToGroupStorage>();
        _isUserExistsSetup = storage.Setup(
            s => s.IsUserExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));

        _isGroupExistsSetup = storage.Setup(
            s => s.IsGroupExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        _getUsersSetup = storage.Setup(
            s => s.GetUsers(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        
        _sut = new AddMemberToGroupUseCase(storage.Object);
    }

    [Fact]
    public async Task ThrowUserNotFoundException_WhenUserIdIsNotExists()
    {
        _isUserExistsSetup.ReturnsAsync(false);

        var actual = _sut.Invoking(sut => sut.Handle(new AddMemberToGroupCommand(
                Guid.Parse("8C955868-F59F-4D63-89DB-B71B8EC00A3C"), 
                Guid.Parse("9E7C6470-CF33-43A0-8924-B29AF753E3B4")), 
            CancellationToken.None));
        
        await actual.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task ThrowGroupNotFoundException_WhenGroupIdIsNotExists()
    {
        _isUserExistsSetup.ReturnsAsync(true);
        _isGroupExistsSetup.ReturnsAsync(false);

        var actual =_sut.Invoking(sut => sut.Handle(new AddMemberToGroupCommand(
                Guid.Parse("4BB1A038-3FF7-441E-A18D-E611515E8BB6"),
                Guid.Parse("52711742-CA83-4B65-A2D7-F1DAF7CFF846")),
            CancellationToken.None));

        await actual.Should().ThrowAsync<GroupNotFoundException>();
    }

    [Fact]
    public async Task ReturnsNotEmptyGroupMembers_WhenUserIdIsExists()
    {
        var groupId = Guid.Parse("DC30C813-E535-4C09-89BA-49FF4EF4BE3F");
        
        _isUserExistsSetup.ReturnsAsync(true);
        _isGroupExistsSetup.ReturnsAsync(true);
        _getUsersSetup.ReturnsAsync(new GroupMembers
        {
            GroupId = groupId,
            Members = new[]
            {
                new GroupMembers.GroupUser
                {
                    UserId = Guid.Parse("21923D22-33B4-41CA-8B3D-10F84CE1D569"),
                    Login = "admin"
                },
                new GroupMembers.GroupUser()
                {
                    UserId = Guid.Parse("EAF804E4-3845-4FBD-8D80-50B8BD35FA82"),
                    Login = "editor"
                }
            }
        });

        var actual = await _sut.Handle(new AddMemberToGroupCommand(
                Guid.Parse("FFF2B945-9403-410E-9C92-AB8873B9E493"),
                Guid.Parse("A5AA70BE-2564-4C6A-90F3-AE97EA00DF5A")),
            CancellationToken.None);

        actual.GroupId.Should().Be(groupId);
        actual.Members.Count().Should().Be(2);
    }
}