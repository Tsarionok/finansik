using Finansik.Domain.UseCases.CreateCategory;
using FluentAssertions;

namespace Finansik.Domain.Tests;

public class CreateCategoryCommandValidatorShould
{
    private readonly CreateCategoryCommandValidator _sut;

    // ReSharper disable once ConvertConstructorToMemberInitializers
    public CreateCategoryCommandValidatorShould()
    {
        _sut = new CreateCategoryCommandValidator();
    }

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var actual = _sut.Validate(
            new CreateCategoryCommand(Guid.Parse("F59161C0-5936-41DC-8655-6496EA084D59"), "Shopping", "shopBug.png"));
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new CreateCategoryCommand(Guid.Parse("26F10649-823A-4DA7-A96A-4602B0EF4270"), "Personal", "person.png");

        yield return [validCommand with { GroupId = Guid.Empty }, nameof(CreateCategoryCommand.GroupId), "Empty"];
        yield return [validCommand with { Name = string.Empty }, nameof(CreateCategoryCommand.Name), "Empty"];
        yield return [validCommand with { GroupId = Guid.Empty, Name = string.Empty }, nameof(CreateCategoryCommand.GroupId), "Empty"];
        yield return [validCommand with { Name = "             " }, nameof(CreateCategoryCommand.Name), "Empty"];
        yield return [validCommand with { Name = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum." }, nameof(CreateCategoryCommand.Name), "TooLong"];
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnFailure_WhenCommandIsInvalid(CreateCategoryCommand command, string expectedInvalidPropertyName, string expectedErrorCode)
    {
        var actual = _sut.Validate(command);
        actual.IsValid.Should().BeFalse();
        actual.Errors.Should()
            .Contain(e => e.PropertyName == expectedInvalidPropertyName && e.ErrorCode == expectedErrorCode);
    }
}