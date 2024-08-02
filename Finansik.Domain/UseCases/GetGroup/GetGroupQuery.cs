using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.GetGroup;

public class GetGroupQuery : IRequest<Group>;