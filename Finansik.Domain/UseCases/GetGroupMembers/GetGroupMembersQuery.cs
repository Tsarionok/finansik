using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroupMembers;

public record GetGroupMembersQuery(Guid GroupId) : IRequest<IEnumerable<GroupMembers>>;