using FluentValidation;

namespace Finansik.Domain.UseCases.GetGroupMembers;

internal class GetGroupMembersQueryValidator : AbstractValidator<GetGroupMembersQuery>
{
    public GetGroupMembersQueryValidator()
    {
        RuleFor(query => query.GroupId).NotEmpty();
    }
}