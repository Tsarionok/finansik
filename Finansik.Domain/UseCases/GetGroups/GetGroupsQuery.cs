using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroups;

public record GetGroupsQuery : IRequest<IEnumerable<Group>>;