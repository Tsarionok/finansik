using FluentValidation;

namespace Finansik.Domain.UseCases.CreateCategory;

internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.GroupId).NotEmpty().WithErrorCode(ValidationErrorCodes.Empty);
        RuleFor(c => c.Name).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCodes.Empty)
            .MaximumLength(30).WithErrorCode(ValidationErrorCodes.TooLong);
    }
}