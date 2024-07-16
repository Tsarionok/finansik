using FluentValidation;

namespace Finansik.Domain.UseCases.CreateCategory;

internal class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.GroupId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(c => c.Name).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(30).WithErrorCode(ValidationErrorCode.TooLong);
    }
}