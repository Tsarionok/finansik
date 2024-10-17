using FluentValidation;

namespace Finansik.Domain.UseCases.RemoveMemberFromGroup;

public class RemoveMemberFromGroupCommandValidator : AbstractValidator<RemoveMemberFromGroupCommand>
{
    public RemoveMemberFromGroupCommandValidator()
    {
        RuleFor(c => c.GroupId).NotEmpty();
        RuleFor(c => c.UserId).NotEmpty();
    }
}