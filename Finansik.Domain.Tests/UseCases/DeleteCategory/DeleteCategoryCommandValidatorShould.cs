using Finansik.Domain.UseCases.DeleteCategory;
using FluentAssertions;
using JetBrains.Annotations;

namespace Finansik.Domain.Tests.UseCases.DeleteCategory;

[TestSubject(typeof(DeleteCategoryCommandValidator))]
public class DeleteCategoryCommandValidatorShould
{
    private readonly DeleteCategoryCommandValidator _sut;

    public DeleteCategoryCommandValidatorShould()
    {
        _sut = new DeleteCategoryCommandValidator();
    }

    [Fact]
    public async Task ReturnSuccess_WhenCommandIsValid()
    {
        var command = new DeleteCategoryCommand(Guid.Parse("91691E2F-4C44-44BF-8E89-4AF6A7434716"));

        var validationResult = await _sut.ValidateAsync(command);
        validationResult.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new DeleteCategoryCommand(Guid.Parse("6F2C4CF6-CAFF-46F2-B72A-2AF173E9F268"));

        yield return [validCommand with { CategoryId = Guid.Empty }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnError_WhenCommandIsInvalid(DeleteCategoryCommand command)
    {
        var validationResult = await _sut.ValidateAsync(command);
        validationResult.IsValid.Should().BeFalse();
    }
}