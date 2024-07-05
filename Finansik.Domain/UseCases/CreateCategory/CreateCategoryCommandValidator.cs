using FluentValidation;
using FluentValidation.Results;

namespace Finansik.Domain.UseCases.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.GroupId).NotEmpty().WithErrorCode("Empty");
        RuleFor(c => c.Name).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(30).WithErrorCode("TooLong");
    }
}