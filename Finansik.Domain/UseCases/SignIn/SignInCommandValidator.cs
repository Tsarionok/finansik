using FluentValidation;

namespace Finansik.Domain.UseCases.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(c => c.Login)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(c => c.Password)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}