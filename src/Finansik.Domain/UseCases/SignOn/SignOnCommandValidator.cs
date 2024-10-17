using FluentValidation;

namespace Finansik.Domain.UseCases.SignOn;

internal sealed class SignOnCommandValidator : AbstractValidator<SignOnCommand>
{
    public SignOnCommandValidator()
    {
        RuleFor(c => c.Login)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);

        RuleFor(c => c.Password)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}