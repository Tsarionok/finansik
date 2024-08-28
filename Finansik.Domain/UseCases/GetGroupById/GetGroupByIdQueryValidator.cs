using FluentValidation;

namespace Finansik.Domain.UseCases.GetGroupById;

internal sealed class GetGroupByIdQueryValidator : AbstractValidator<GetGroupByIdQuery>
{
    public GetGroupByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotNull()
            .NotEmpty();
    }
}