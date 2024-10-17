using FluentValidation;

namespace Finansik.Domain.UseCases.AddMemberToGroup;

internal sealed class AddMemberToGroupCommandValidator : AbstractValidator<AddMemberToGroupCommand>
{
    public AddMemberToGroupCommandValidator()
    {
        RuleFor(command => command.GroupId).NotEmpty();
        RuleFor(command => command.UserId).NotEmpty();
    }
}