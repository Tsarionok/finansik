using FluentValidation;

namespace Finansik.Domain.UseCases.GetGroup;

internal sealed class GetGroupQueryValidator : AbstractValidator<GetGroupQuery>
{
    public GetGroupQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotNull()
            .NotEmpty();
    }
}