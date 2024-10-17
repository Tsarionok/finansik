using FluentValidation;

namespace Finansik.Domain.UseCases.DeleteCategory;

internal sealed class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(c => c.CategoryId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
    }
}