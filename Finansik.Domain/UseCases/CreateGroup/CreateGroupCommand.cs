using Finansik.Domain.Models;
using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.CreateGroup;

public record CreateGroupCommand(string Name, string? Icon) : 
    DefaultMonitoredRequest("groups.created"), IRequest<Group>;