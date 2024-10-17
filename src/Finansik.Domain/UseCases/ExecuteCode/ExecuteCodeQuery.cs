using Finansik.Domain.Monitoring;
using MediatR;

namespace Finansik.Domain.UseCases.ExecuteCode;

public record ExecuteCodeQuery(string Code, string StartMethod, int Argument) 
    : DefaultMonitoredRequest("code.executed"), IRequest<ExecuteCodeQueryResult>;