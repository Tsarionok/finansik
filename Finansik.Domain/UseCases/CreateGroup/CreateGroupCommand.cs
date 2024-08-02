using Finansik.Domain.Models;
using MediatR;

namespace Finansik.Domain.UseCases.CreateGroup;

public record CreateGroupCommand(string Name, string? Icon) : IRequest<Group>;