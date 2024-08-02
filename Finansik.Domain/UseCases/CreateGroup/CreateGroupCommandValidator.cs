using FluentValidation;

namespace Finansik.Domain.UseCases.CreateGroup;

internal class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(command => command.Name).NotNull().NotEmpty();
    }
}